CREATE TABLE [Engineers] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Firstname] [nchar](50) NOT NULL,
	[Lastname] [nchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT (1),
	CONSTRAINT [PK_Engineers] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [Shifts](
	[Id] [smallint] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[EndTime] [time](7) NOT NULL,
	CONSTRAINT [PK_Shifts] PRIMARY KEY CLUSTERED ([Id] ASC)
)

CREATE TABLE [Schedules] (
	[Date] [date] NOT NULL,
	[Shift] [smallint] NOT NULL,
	[EngineerId] [int] NOT NULL,
	CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED ([Date] ASC, [Shift] ASC),
	CONSTRAINT [FK_Schedules_Engineers] FOREIGN KEY ([EngineerId]) REFERENCES [Engineers] ([Id]),
	CONSTRAINT [FK_Schedules_Shifts] FOREIGN KEY ([Shift]) REFERENCES [Shifts] ([Id])
)
