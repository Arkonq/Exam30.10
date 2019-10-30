Create table AcceptedWaybills
(
[Id] uniqueidentifier not null primary key,
[CreationDate] datetime not null,
[StoreId] uniqueidentifier not null,
[StorekeeperId] uniqueidentifier not null,
[WaybillId] uniqueidentifier not null,
)