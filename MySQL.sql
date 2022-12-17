USE noCarbon;
DELIMITER $$
CREATE DEFINER=`appuser`@`localhost` PROCEDURE `GetBalance`(pCustomerId CHAR(36))
BEGIN  
   SELECT COALESCE(SUM(POINTS),0) as Balance, COALESCE(SUM(ReducedCarb),0) as TotalImpact FROM Historic 
    WHERE CustomerId = pCustomerId;  
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`appuser`@`localhost` PROCEDURE `GetHistoric`(pCustomerId CHAR(36), pCategoryId CHAR(36),pActionId CHAR(36))
BEGIN  
   SELECT H.CustomerId, c.Name as CategoryName, A.Name as ActionName,COALESCE(H.Points,0) AS Points,COALESCE(H.ReducedCarb,0) AS ReducedCarb, H.OperationDate, H.OperationTime FROM Historic H
	INNER JOIN Category c ON H.CategoryId = c.Id
	INNER JOIN Actions A ON H.ActionId = A.Id
     -- add where condition if required
	WHERE H.CustomerId = pCustomerId AND (H.CategoryId = CASE WHEN ((pCategoryId IS NULL) OR (pCategoryId = '')) THEN H.CategoryId ELSE pCategoryId END)
    AND (H.ActionId = CASE WHEN ((pActionId IS NULL) OR (pActionId = '')) THEN H.ActionId ELSE pActionId END);  
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`appuser`@`localhost` PROCEDURE `GetLeaderboard`()
BEGIN  
	SET @row_number = 0;
	SELECT @row_number := @row_number + 1 AS Classement, H.CustomerId, c.UserName, COALESCE(SUM(H.POINTS),0) as Balance, COALESCE(SUM(H.ReducedCarb),0) as TotalImpact FROM Historic H
	INNER JOIN Customer c ON H.CustomerId = c.Id
    ORDER BY TotalImpact;
END$$
DELIMITER ;
DELIMITER $$
CREATE DEFINER=`appuser`@`localhost` PROCEDURE `GetMyWeeklyTrend`(pCustomerId CHAR(36))
BEGIN  
SELECT DATE(D.DATEDAY) AS DayOfTheWeek, COALESCE(SUM(H.ReducedCarb),0) as TotalImpact FROM (SELECT DATE_SUB(NOW(), INTERVAL D DAY) AS DATEDAY FROM (SELECT 0 as D  UNION SELECT 1  UNION SELECT 2 UNION SELECT 3 UNION SELECT 4  UNION SELECT 5 
UNION SELECT 6 ) AS DATEDAY) AS D
LEFT JOIN Historic H
ON DATE(H.OperationDate) = date(D.DATEDAY) AND H.CustomerId = pCustomerId
GROUP BY DATE(D.DATEDAY)
ORDER BY D.DATEDAY ASC;
END$$
DELIMITER ;