-- Make Names table.
DROP TABLE IF EXISTS `names`;
CREATE TABLE `names` (
	`name_id` INTEGER NOT NULL AUTO_INCREMENT,
    `name` TINYTEXT NOT NULL,
    `birthday` DATE NOT NULL,
    `address` TEXT NOT NULL,
    `phonenumber` BIGINT NOT NULL,
    
    PRIMARY KEY (`name_id`)
);
-- Make Positions table.
DROP TABLE IF EXISTS `positions`;
CREATE TABLE `positions` (
	`position_id` INTEGER NOT NULL AUTO_INCREMENT,
    `title` TINYTEXT NOT NULL,

    PRIMARY KEY (`position_id`)
);

-- Make Departments table.
DROP TABLE IF EXISTS `departments`;
CREATE TABLE `departments` (
	`department_id` INTEGER NOT NULL AUTO_INCREMENT,
    `title` TINYTEXT NOT NULL,
    
    PRIMARY KEY (`department_id`)
);

-- Make Employees table.
DROP TABLE IF EXISTS `employees`;
CREATE TABLE `employees` (
	`employee_id` INTEGER NOT NULL AUTO_INCREMENT,
	`name_id` INTEGER NOT NULL,
    `position_id` INTEGER NOT NULL,
    `supervisor_id` INTEGER,
    `department_id` INTEGER NOT NULL,
    `addeddate` TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

	PRIMARY KEY (`employee_id`),
    FOREIGN KEY (`name_id`) REFERENCES `names`(`name_id`),
    FOREIGN KEY (`position_id`) REFERENCES `positions`(`position_id`),
    FOREIGN KEY (`department_id`) REFERENCES `departments`(`department_id`)
);

SET GLOBAL log_bin_trust_function_creators = 1;

DELIMITER $$

-- Create a function to add employee.
-- The returned value indicates new employee id.
-- If failed, then return code is negative and:
--  -100 - indicates that position was not found.
--  -200 - indicates that department was not found.
--  -300 - indicates that insertion operation was not successfull.
DROP FUNCTION IF EXISTS AddEmployeeFunc$$
CREATE FUNCTION AddEmployeeFunc(
	`name` TINYTEXT,
    `birthday` DATE,
    `position` INT,
    `phonenumber` BIGINT,
    `department` INT,
    `address` TEXT,
    `IsSupervisor` BOOL
)
RETURNS INT
BEGIN
	DECLARE `nameIdOld` INT;
	DECLARE `nameId` INT;
    DECLARE `employeeIdOld` INT;
    DECLARE `employeeId` INT;
    DECLARE `supervisorId` INT;

	-- Check if position is valid.
    if NOT EXISTS(SELECT `position_id` FROM `positions` WHERE `positions`.`position_id` = `position`) THEN
		RETURN -100;
	END IF;
    
    -- Check if department is valid.
    IF NOT EXISTS(SELECT `department_id` FROM `departments` WHERE `departments`.`department_id` = `department`) THEN
		RETURN -200;
	END IF;
    
    -- Get supervisor for selected department.
    IF `IsSupervisor` = TRUE THEN
		SET `supervisorId` = 0;
	ELSE
		SELECT `employee_id` INTO `supervisorId` FROM `employees` WHERE `department_id` = `department` AND ISNULL(`supervisor_id`) ORDER BY `employee_id` DESC LIMIT 1;
	END IF;
    
    -- Employees table depends on data that should exist in other tables. Names table goes first.
    SELECT `name_id` INTO `nameIdOld` FROM `names` ORDER BY `name_id` DESC LIMIT 1;
    INSERT INTO `names` (`name`, `birthday`, `address`, `phonenumber`) VALUES
		(`name`, `birthday`, `address`, `phonenumber`);
	SELECT `name_id` INTO `nameId` FROM `names` ORDER BY `name_id` DESC LIMIT 1;

    if `nameIdOld` = `nameId` THEN
		RETURN -300;
	END IF;
    
    -- Got name id, so now can insert data into employees table.
    SELECT `employee_id` INTO `employeeIdOld` FROM `employees` ORDER BY `employee_id` DESC LIMIT 1;
    INSERT INTO `employees` (`name_id`, `position_id`, `supervisor_id`, `department_id`) VALUES
		(`nameId`, `position`, `supervisorId`, `department`);
	SELECT `employee_id` INTO `employeeId` FROM `employees` ORDER BY `employee_id` DESC LIMIT 1;
    
    if `employeeIdOld` = `employeeId` THEN
        RETURN -400;
	END IF;
    
    RETURN `employeeId`;
END$$

-- Create a procedure to add new department.
DROP PROCEDURE IF EXISTS AddDepartment$$
CREATE PROCEDURE AddDepartment(
	IN `newTitle` TINYTEXT,

	OUT `result` INT
)
BEGIN
	IF EXISTS(SELECT `department_id` FROM `departments` WHERE `title` = `newTitle`) THEN
		SET `result` = 0;
	ELSE
		INSERT INTO `departments` (`title`) VALUES (`newTitle`);
        SET `result` = 1;
	END IF;
END$$

-- Create a procedure to add new position.
DROP PROCEDURE IF EXISTS AddPosition$$
CREATE PROCEDURE AddPosition(
	IN `newTitle` TINYTEXT,

	OUT `result` INT
)
BEGIN
	IF EXISTS(SELECT `position_id` FROM `positions` WHERE `title` = `newTitle`) THEN
		SET `result` = 0;
	ELSE
		INSERT INTO `positions` (`title`) VALUES (`newTitle`);
        SET `result` = 1;
	END IF;
END$$

-- Create a procedure to add employee.
DROP PROCEDURE IF EXISTS AddEmployee$$
CREATE PROCEDURE AddEmployee(
	IN `name` TINYTEXT,
    IN `birthday` DATE,
    IN `position` INT,
    IN `phonenumber` BIGINT,
    IN `department` INT,
    IN `address` TEXT,
    IN `IsSupervisor` BOOL,

	OUT `result` INT
)
BEGIN
	SET `result` = AddEmployeeFunc(`name`, `birthday`, `position`, `phonenumber`, `department`, `address`, `IsSupervisor`);
END$$

-- Create a procedure that can retrieve all available positions.
DROP PROCEDURE IF EXISTS GetPositions$$
CREATE PROCEDURE GetPositions()
BEGIN
	SELECT * FROM `positions` ORDER BY `position_id` ASC;
END$$

-- Create a procedure that can retrieve all available departments.
DROP PROCEDURE IF EXISTS GetDepartments$$
CREATE PROCEDURE GetDepartments()
BEGIN
	SELECT * FROM `departments` ORDER BY `department_id` ASC;
END$$

-- Create a procedure that can retrieve all supervisors.
DROP PROCEDURE IF EXISTS GetSupervisors$$
CREATE PROCEDURE GetSupervisors()
BEGIN
	SELECT `employee_id`, `names`.`name` FROM `employees`
		RIGHT JOIN `names` ON (`employees`.`name_id` = `names`.`name_id`)
        WHERE ISNULL(`employees`.`supervisor_id`)
        GROUP BY `names`.`name`;
END$$

-- Create a procedure that returns supervisor information for specified department.
DROP PROCEDURE IF EXISTS GetSupervisorForDepartment$$
CREATE PROCEDURE GetSupervisorForDepartment(
	IN `departmentId` INT,

	OUT `result` INT
)
BEGIN
	SELECT `employees`.`employee_id` INTO `result` FROM `employees`
    WHERE
		`department_id` = `departmentId` AND ISNULL(`supervisor_id`)
	LIMIT 1;
END$$

-- Create a procedure that updates employee data.
-- Returns true on success, otherwise:
--   -400 - if employee id specified was not found.
DROP PROCEDURE IF EXISTS UpdateEmployee$$
CREATE PROCEDURE UpdateEmployee(
	IN `employeeId` INT,
	IN `newName` TINYTEXT,
    IN `newBirthday` DATE,
    IN `newPosition` INT,
    IN `newPhoneNumber` BIGINT,
    IN `newDepartment` INT,
    IN `newAddress` TEXT,
    IN `newIsSupervisor` BOOL,
    
    OUT `result` INT
)
RoutineLabel:
BEGIN
	DECLARE `supervisorId` INT;

	-- No such employee - return.
	IF NOT EXISTS(SELECT `employee_id` FROM `employees` WHERE `employee_id` = `employeeId`) THEN
		SET `result` = -400;
        LEAVE RoutineLabel;
	END IF;
    
    UPDATE `names`
    SET
		`name` = `newName`,
        `birthday` = `newBirthday`,
        `phonenumber` = `newPhoneNumber`,
        `address` = `newAddress`
	WHERE
		`name_id` = (SELECT `name_id` FROM `employees` WHERE `employee_id` = `employeeId`);

	CALL GetSupervisorForDepartment(`newDepartment`, `supervisorId`);

	UPDATE `employees`
    SET
		`position_id` = `newPosition`,
        `department_id` = `newDepartment`,
        `supervisor_id` = IF(`newIsSupervisor` != 0, 0, `supervisorId`)
	WHERE
		`employee_id` = `employeeId`;

	SET `result` = 1;
END$$

-- Create a procedure that removes selected employee.
DROP PROCEDURE IF EXISTS RemoveEmployee$$
CREATE PROCEDURE RemoveEmployee(
	IN `employeeId` INT,

	OUT `result` INT
)
BEGIN
	DECLARE `nameId` INT;

	SELECT `name_id` INTO `nameId` FROM `names` WHERE `name_id` = (SELECT `name_id` FROM `employees` WHERE `employee_id` = `employeeId`);

    UPDATE `employees` SET
		`supervisor_id` = 0
	WHERE
		`employees`.`supervisor_id` = `employeeId`;

    DELETE FROM `employees` WHERE `employee_id` = `employeeId`;
    DELETE FROM `names` WHERE `names`.`name_id` = `nameId`;
    
    SET `result` = 1;
END$$

-- Create a procedure that can retrieve all employees.
-- If no filter is specified, then plain list is returned.
DROP PROCEDURE IF EXISTS GetEmployees$$
CREATE PROCEDURE GetEmployees(
	IN `department` INT,
    IN `supervisor` INT
)
BEGIN
	IF `department` = -1 THEN
		SET `department` = NULL;
	END IF;

    IF `supervisor` = -1 THEN
		SET `supervisor` = NULL;
	END IF;

    SELECT
		`employees`.`employee_id`,
		`namesEmployee`.`name`,
        `namesEmployee`.`birthday`,
        `namesEmployee`.`address`,
        `namesEmployee`.`phonenumber`,
        `positions`.`title` AS `position_title`,
        `departments`.`title` AS `department_title`,
        `supervisors`.`name` AS `supervisor_name`
	FROM `employees`
    INNER JOIN `names` AS `namesEmployee` ON (`namesEmployee`.`name_id` = `employees`.`name_id`)
    INNER JOIN `departments` ON (`departments`.`department_id` = `employees`.`department_id`)
    INNER JOIN `positions` ON (`positions`.`position_id` = `employees`.`position_id`)
    LEFT JOIN (
		SELECT `employeesInner`.`employee_id`, `namesInner`.`name` FROM `employees` AS `employeesInner`
		RIGHT JOIN `names` AS `namesInner` ON (`employeesInner`.`name_id` = `namesInner`.`name_id`)
        WHERE ISNULL(`employeesInner`.`supervisor_id`)
    ) AS `supervisors` ON (`supervisors`.`employee_id` = `employees`.`supervisor_id`)
    WHERE
		(`department` IS NULL OR `employees`.`department_id` = `department`) AND
        (`supervisor` IS NULL OR `employees`.`supervisor_id` = `supervisor`)
    ORDER BY `employees`.`employee_id` ASC;
END$$

-- Create a procedure that retrieves selected employee data.
DROP PROCEDURE IF EXISTS GetEmployee$$
CREATE PROCEDURE GetEmployee(
	IN `employeeId` INT
)
BEGIN
	SELECT
		`employees`.`employee_id`,
        `namesEmployee`.`name`,
        `namesEmployee`.`birthday`,
        `employees`.`position_id`,
        `namesEmployee`.`phonenumber`,
        `employees`.`department_id`,
        `employees`.`supervisor_id`,
        `namesEmployee`.`address`
	FROM `employees`
    INNER JOIN `names` AS `namesEmployee` ON (`namesEmployee`.`name_id` = `employees`.`name_id`)
    WHERE
		`employees`.`employee_id` = `employeeId`;
END$$

DELIMITER ;