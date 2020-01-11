CREATE TABLE [dbo].[EventTeamMatch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EventKey] [nvarchar](50) NULL,
	[TeamNumber] [int] NULL,
	[MatchNumber] [int] NULL,
	[AutoStartPos] [int] NULL,
	[AutoLeaveInitLine] [int] NULL,
	[AutoLowCell] [int] NULL,
	[AutoHighCell] [int] NULL,
	[AutoInnerCell] [int] NULL,
	[TeleLowCell] [int] NULL,
	[TeleHighCell] [int] NULL,
	[TeleInnerCell] [int] NULL,
	[RotationControl] [int] NULL,
	[PositionControl] [int] NULL,
	[ClimbStatus] [int] NULL,
	[LevelSwitch] [int] NULL,
	[Fouls] [int] NULL,
	[Broken] [int] NULL,
	[AllianceResult] [int] NULL,
	[StageRankingPoint] [int] NULL,
	[ClimbRankingPoint] [int] NULL,
	[ScouterName] [nvarchar](50) NULL,
	[Comments] [nvarchar](max) NULL,
 CONSTRAINT [PK_EventTeamMatch] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_EventTeamMatch_EventKey_TeamNumber_MatchNumber] ON [dbo].[EventTeamMatch]
(
	[EventKey] ASC,
	[TeamNumber] ASC,
	[MatchNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
