/* Engineers */
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Charlot', 'Eede');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Natalee', 'Wigin');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Nap', 'Nimmo');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Auria', 'De Witt');
INSERT [Engineers] ([Firstname], [Lastname], [IsActive]) VALUES ('Ginny', 'Oxbrow', 0);
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Burk', 'Maciaszek');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Hetti', 'Whatley');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Monti', 'Vercruysse');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Lethia', 'Bonefant');
INSERT [Engineers] ([Firstname], [Lastname], [IsActive]) VALUES ('Yevette', 'Lawlings', 0);
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Sharity', 'Dronsfield');
INSERT [Engineers] ([Firstname], [Lastname]) VALUES ('Nicolea', 'Appleyard');

/* Shifts */
INSERT [Shifts] ([Id], [StartTime], [EndTime]) VALUES (1, CAST(N'09:00:00' AS Time), CAST(N'13:00:00' AS Time))
INSERT [Shifts] ([Id], [StartTime], [EndTime]) VALUES (2, CAST(N'13:00:00' AS Time), CAST(N'17:00:00' AS Time))
