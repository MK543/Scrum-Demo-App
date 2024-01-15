INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('L', 'L', 'L@gmail.com', 'Admin123!', 0);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('M', 'M', 'M@gmail.com', 'Admin123!',0);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('John', 'Doe', 'john@example.com', '"Password"1',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Alice', 'Smith', 'alice@example.com', '"Password"2',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Bob', 'Johnson', 'bob@example.com', '"Password"3',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Emily', 'Brown', 'emily@example.com', '"Password"4',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Michael', 'Davis', 'michael@example.com', '"Password"5',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Emma', 'Wilson', 'emma@example.com', '"Password"6',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('William', 'Clark', 'william@example.com', '"Password"7',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Sophia', 'Anderson', 'sophia@example.com', '"Password"8',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Oliver', 'Martinez', 'oliver@example.com', '"Password"9',1);
INSERT INTO "Users" ("FirstName", "LastName", "Email", "Password","JobTitle") VALUES ('Ava', 'Garcia', 'ava@example.com', '"Password"10',1);

INSERT INTO "Projects" ("ProjectName","ProjectDescription","CreationDate", "FinishDate", "ProjectCreatorId") VALUES ('Project1','Dummy Desc2','2023-04-05T14:00:00+00:00', '2023-05-05T16:30:00+00:00',1);
INSERT INTO "Projects" ("ProjectName","ProjectDescription","CreationDate", "FinishDate", "ProjectCreatorId") VALUES ('Project2','Dummy Desc2','2023-06-10T09:15:00+00:00', '2023-07-10T11:45:00+00:00',1);
INSERT INTO "Projects" ("ProjectName","ProjectDescription","CreationDate", "FinishDate", "ProjectCreatorId") VALUES ('Project3','Dummy Desc2','2023-08-25T20:30:00+00:00', '2023-09-25T22:15:00+00:00',2);

INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task1', 'Des1', 3, 1,1,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task2', 'Des2', 3, 2,2,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task3', 'Des3', 3, 3,3,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task4', 'Des4', 3, 1,1,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task5', 'Des5', 3, 2,2,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task6', 'Des6', 3, 3,3,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task7', 'Des7', 3, 1,1,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task8', 'Des8', 3, 2,2,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task9', 'Des9', 3, 3,3,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task10', 'Des10', 3, 1,1,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task11', 'Des11', 3, 2,2,1);
INSERT INTO "Tasks" ("Name", "Description", "TaskType", "ProjectId","Points","AssignedUserId") VALUES ('Task12', 'Des12', 3, 3,3,1);


INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (1, 1);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (1, 2);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (1, 3);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (2, 1);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (2, 2);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (2, 3);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (3, 1);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (4, 2);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (5, 3);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (6, 1);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (7, 2);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (8, 3);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (9, 1);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (10, 2);
INSERT INTO "ProjectUser" ("UsersId", "ProjectsId") VALUES (11, 3);