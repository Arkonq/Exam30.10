Create table Waybills
(
[Id] uniqueidentifier not null primary key,
[StoreId] uniqueidentifier not null,
[Apples] int null,
[Bananas] int null,
[Pears] int null,
[Action] nvarchar(max) not null
)