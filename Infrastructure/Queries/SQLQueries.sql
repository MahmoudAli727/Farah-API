--Password = "M@m123456789"

-- Insert 10 customers into AspNetUsers and Customers tables
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, SSN, GovID, CityID, ProfileImage, IsBlocked, IsDeleted, YourFavirotePerson)
VALUES
    (NEWID(), 'customer1', 'CUSTOMER1', 'c1@c.com', 'C1@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567890', 0, 0, NULL, 1, 0, 'Customer', 'Johnson', '123-45-6789', 1, 1, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer2', 'CUSTOMER2', 'c2@c.com', 'C2@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567891', 0, 0, NULL, 1, 0, 'Customer', 'Williams', '123-45-6790', 2, 2, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer3', 'CUSTOMER3', 'c3@c.com', 'C3@C.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567892', 0, 0, NULL, 1, 0, 'Customer', 'Brown', '123-45-6791', 3, 3, '/images/CustomersImages/defualt-custoemr-img.png', 1, 0, 'answer'),
    (NEWID(), 'customer4', 'CUSTOMER4', 'c4@c.com', 'C4@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567893', 0, 0, NULL, 1, 0, 'Customer', 'Taylor', '123-45-6792', 4, 4, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer5', 'CUSTOMER5', 'c5@c.com', 'C5@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567894', 0, 0, NULL, 1, 0, 'Customer', 'Martinez', '123-45-6793', 5, 5, '/images/CustomersImages/defualt-custoemr-img.png', 1, 0, 'answer'),
    (NEWID(), 'customer6', 'CUSTOMER6', 'c6@c.com', 'C6@C.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567895', 0, 0, NULL, 1, 0, 'Customer', 'Garcia', '123-45-6794', 6, 6, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer7', 'CUSTOMER7', 'c7@c.com', 'C7@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567896', 0, 0, NULL, 1, 0, 'Customer', 'Miller', '123-45-6795', 7, 7, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer8', 'CUSTOMER8', 'c8@c.com', 'C8@C.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567897', 0, 0, NULL, 1, 0, 'Customer', 'Martinez', '123-45-6796', 8, 8, '/images/CustomersImages/defualt-custoemr-img.png', 1, 0, 'answer'),
    (NEWID(), 'customer9', 'CUSTOMER9', 'c9@c.com', 'C9@C.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567898', 0, 0, NULL, 1, 0, 'Customer', 'Hernandez', '123-45-6797', 9, 9, '/images/CustomersImages/defualt-custoemr-img.png', 0, 0, 'answer'),
    (NEWID(), 'customer10', 'CUSTOMER10', 'c10@c.com', 'C10@C.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567899', 0, 0, NULL, 1, 0, 'Customer', 'Lopez', '123-45-6798', 10, 10, '/images/CustomersImages/defualt-custoemr-img.png', 1, 0, 'answer');

-- Insert into Customers table
INSERT INTO Customers (Id)
SELECT Id FROM AspNetUsers WHERE UserName IN ('customer1', 'customer2', 'customer3', 'customer4', 'customer5', 'customer6', 'customer7', 'customer8', 'customer9', 'customer10');

-- Insert 20 owners into AspNetUsers and Owners tables
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName, SSN, GovID, CityID, ProfileImage, IsBlocked, IsDeleted, YourFavirotePerson)
VALUES
    (NEWID(), 'owner1', 'OWNER1', 'o1@o.com', 'O1@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567800', 0, 0, NULL, 1, 0, 'Owner', 'Smith', '123-45-6800', 1, 1, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3e.webp', 1, 0, 'answer'),
    (NEWID(), 'owner2', 'OWNER2', 'o2@o.com', 'O2@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567801', 0, 0, NULL, 1, 0, 'Owner', 'Johnson', '123-45-6801', 2, 2, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3f.webp', 0, 0, 'answer'),
    (NEWID(), 'owner3', 'OWNER3', 'o3@o.com', 'O3@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567802', 0, 0, NULL, 1, 0, 'Owner', 'Brown', '123-45-6802', 3, 3, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3g.webp', 1, 0, 'answer'),
    (NEWID(), 'owner4', 'OWNER4', 'o4@o.com', 'O4@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567803', 0, 0, NULL, 1, 0, 'Owner', 'Taylor', '123-45-6803', 4, 4, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3h.webp', 0, 0, 'answer'),
    (NEWID(), 'owner5', 'OWNER5', 'o5@o.com', 'O5@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567804', 0, 0, NULL, 1, 0, 'Owner', 'Martinez', '123-45-6804', 5, 5, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3i.webp', 1, 0, 'answer'),
    (NEWID(), 'owner6', 'OWNER6', 'o6@o.com', 'O6@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567805', 0, 0, NULL, 1, 0, 'Owner', 'Garcia', '123-45-6805', 6, 6, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3j.webp', 0, 0, 'answer'),
    (NEWID(), 'owner7', 'OWNER7', 'o7@o.com', 'O7@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567806', 0, 0, NULL, 1, 0, 'Owner', 'Miller', '123-45-6806', 7, 7, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3k.webp', 1, 0, 'answer'),
    (NEWID(), 'owner8', 'OWNER8', 'o8@o.com', 'O8@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567807', 0, 0, NULL, 1, 0, 'Owner', 'Martinez', '123-45-6807', 8, 8, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3l.webp', 0, 0, 'answer'),
    (NEWID(), 'owner9', 'OWNER9', 'o9@o.com', 'O9@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567808', 0, 0, NULL, 1, 0, 'Owner', 'Hernandez', '123-45-6808', 9, 9, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3m.webp', 1, 0, 'answer'),
    (NEWID(), 'owner10', 'OWNER10', 'o10@o.com', 'O10@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567809', 0, 0, NULL, 1, 0, 'Owner', 'Lopez', '123-45-6809', 10, 10, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3n.webp', 1, 0, 'answer'),
    (NEWID(), 'owner11', 'OWNER11', 'o11@o.com', 'O11@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567810', 0, 0, NULL, 1, 0, 'Owner', 'Brown', '123-45-6810', 11, 11, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3o.webp', 1, 0, 'answer'),
    (NEWID(), 'owner12', 'OWNER12', 'o12@o.com', 'O12@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567811', 0, 0, NULL, 1, 0, 'Owner', 'Garcia', '123-45-6811', 12, 12, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3p.webp', 0, 0, 'answer'),
    (NEWID(), 'owner13', 'OWNER13', 'o13@o.com', 'O13@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567812', 0, 0, NULL, 1, 0, 'Owner', 'Miller', '123-45-6812', 13, 13, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3q.webp', 1, 0, 'answer'),
    (NEWID(), 'owner14', 'OWNER14', 'o14@o.com', 'O14@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567813', 0, 0, NULL, 1, 0, 'Owner', 'Martinez', '123-45-6813', 14, 14, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3r.webp', 0, 0, 'answer'),
    (NEWID(), 'owner15', 'OWNER15', 'o15@o.com', 'O15@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567814', 0, 0, NULL, 1, 0, 'Owner', 'Hernandez', '123-45-6814', 15, 15, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3s.webp', 1, 0, 'answer'),
    (NEWID(), 'owner16', 'OWNER16', 'o16@o.com', 'O16@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567815', 0, 0, NULL, 1, 0, 'Owner', 'Lopez', '123-45-6815', 16, 16, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3t.webp', 0, 0, 'answer'),
    (NEWID(), 'owner17', 'OWNER17', 'o17@o.com', 'O17@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567816', 0, 0, NULL, 1, 0, 'Owner', 'Brown', '123-45-6816', 17, 17, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3u.webp', 1, 0, 'answer'),
    (NEWID(), 'owner18', 'OWNER18', 'o18@o.com', 'O18@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567817', 0, 0, NULL, 1, 0, 'Owner', 'Garcia', '123-45-6817', 18, 18, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3v.webp', 0, 0, 'answer'),
    (NEWID(), 'owner19', 'OWNER19', 'o19@o.com', 'O19@O.COM', 1, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567818', 0, 0, NULL, 1, 0, 'Owner', 'Miller', '123-45-6818', 19, 19, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3w.webp', 1, 0, 'answer'),
    (NEWID(), 'owner20', 'OWNER20', 'o20@o.com', 'O20@O.COM', 0, 'AQAAAAIAAYagAAAAEMYE8POdgfZKD9hh1AvqBndKxuq4CZUCyH5NwibDvk24vuiGLnxxdTajMkd7VK97VA==', NEWID(), NEWID(), '1234567819', 0, 0, NULL, 1, 0, 'Owner', 'Martinez', '123-45-6819', 20, 20, '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3x.webp', 0, 0, 'answer');

-- Insert into Owners table
INSERT INTO Owners (Id, IDFrontImage, IDBackImage, UserType, AccountStatus)
SELECT 
    u.Id, 
    '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3e.webp' AS IDFrontImage, 
    '/images/OwnersImages/7875f4d8-4d73-43e0-a290-35c60a098a3e.webp' AS IDBackImage, 
    0 AS UserType, 
    0 AS AccountStatus 
FROM 
    AspNetUsers u
WHERE 
    u.UserName IN (
        'owner1', 'owner2', 'owner3', 'owner4', 'owner5', 
        'owner6', 'owner7', 'owner8', 'owner9', 'owner10', 
        'owner11', 'owner12', 'owner13', 'owner14', 'owner15', 
        'owner16', 'owner17', 'owner18', 'owner19', 'owner20'
    );

-- Insert owners into AspNetUserRoles
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, 'ca53e52b-5c38-4366-b5a3-0c259ecbec72' -- RoleId for owner role
FROM AspNetUsers u
JOIN Owners o ON o.Id = u.Id;

-- Insert customers into AspNetUserRoles
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, '0ed39936-437c-4651-b4d4-4678e280d327' -- RoleId for customer role
FROM AspNetUsers u
JOIN Customers c ON c.Id = u.Id;
