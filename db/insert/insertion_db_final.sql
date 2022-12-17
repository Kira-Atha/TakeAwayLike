INSERT INTO dbo.Customers (FirstName,LastName,Password,Gender,Address,PostalCode,City,Country,Email,PhoneNumber,DateOfBirth) VALUES 
('Arnaud','Verber','UUqat8IaVqYKOa8nJaRkAauQR1fc6kApZuvJFGGFenA=','M','rue enghien 12','6200','Chatelet','Belgique','arnaud.verbeer@gmail.com','0479854147','1996-06-06')


INSERT INTO dbo.Restorers(FirstName,LastName,Password,Gender,Address,PostalCode,City,Country,Email,PhoneNumber) VALUES
('Roland','Vagand','UUqat8IaVqYKOa8nJaRkAauQR1fc6kApZuvJFGGFenA=','M','rue du vagabond 18','6000','Charleroi','Belgique','roland.vagand@gmail.com','0479781295')


INSERT INTO dbo.Restaurants(Name,Adress,PostalCode,City,Country,PhoneNumber,RestaurantType,Description,TvaNumber,RestorerId) 
VALUES
('Le catalan','place verte 3','6000','Charleroi','Belgique','071323768',7,'Restaurant espagnol','BE7123543235',1)


INSERT INTO dbo.Schedules (RestaurantId,OpeningDay,OpenTime,CloseTime) VALUES
(1,'Lundi','12:00:00','23:00:00'),
(1,'Mardi','12:00:00','23:00:00'),
(1,'Mercredi','12:00:00','23:00:00'),
(1,'Jeudi','12:00:00','23:00:00'),
(1,'Vendredi','12:00:00','23:00:00'),
(1,'Samedi','11:30:00','23:00:00'),
(1,'Dimanche','11:30:00','23:00:00')


INSERT INTO dbo.Dishes (RestaurantId,Name,Price,TypeService,TypeDish,Description) VALUES
(1,'Paella',25.0,0,0,'paella fruits de mers'),
(1,'Homard',50.0,1,0,'Homard frais'),
(1,'Gaspacho',15,0,0,'Gaspacho avec recette traditionelle'),
(1,'Churros',6,0,2,'Churros maison'),
(1,'Sangria',6,1,3,'Sangria maison')


INSERT INTO dbo.Menus (RestaurantId,Name,Price,TypeService,Description) VALUES
(1,'Menu Paella et Homard',75.0,1,'Menu avec : paella,homard,sangria et churros'),
(1,'Menu Gaspacho',25.0,0,'Menu gaspacho avec sangria et churros')


INSERT INTO dbo.MenuDetails(MenuId,DishId) VALUES 
(1,1),(1,2),(1,4),(1,5),
(2,3),(2,4),(2,5)


INSERT INTO dbo.Orders (OrderStatus,DeliveryAdress,OrderDate,Price,Note,CustomerId,RestaurantId) VALUES
(0,'rue enghien 12,Chatelet 6200','2021-05-26',81,null,1,1),
(0,'rue enghien 12,Chatelet 6200','2021-05-26',106,null,1,1)

INSERT INTO dbo.OrderMenuDetails(OrderId,MenuId) VALUES
(1,1),
(2,1),(2,2)

INSERT INTO dbo.OrderDishDetails (OrderId,DishId) VALUES
(1,4),
(2,5)






