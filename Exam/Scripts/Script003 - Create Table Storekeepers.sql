Create table Storekeepers
(
[Id] uniqueidentifier not null primary key,
[StoreId] uniqueidentifier not null,
[Name] nvarchar(max) not null,
[Pass] nvarchar(max) not null
)