import SyntaxHighlighter from 'react-syntax-highlighter';
import { atomOneDark } from 'react-syntax-highlighter/dist/esm/styles/hljs';
import { Card, Alert } from 'react-bootstrap';

export function FilmDbScheme() {
    const sqlCode = `
-- Таблица фильмов
CREATE TABLE [dbo].[Films](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    [Description] [nvarchar](200) NULL,
    CONSTRAINT [PK_Films] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- Таблица залов
CREATE TABLE [dbo].[Halls](
    [Id] [tinyint] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Halls] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- Таблица сеансов
CREATE TABLE [dbo].[Screenings](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [HallId] [tinyint] NOT NULL,
    [FilmId] [bigint] NOT NULL,
    [Time] [datetime2](7) NOT NULL,
    CONSTRAINT [PK_Screenings] PRIMARY KEY CLUSTERED ([Id] ASC)
)

-- Внешние ключи для таблицы сеансов
ALTER TABLE [dbo].[Screenings] WITH CHECK ADD CONSTRAINT [FK_Screenings_Films_FilmId] 
FOREIGN KEY([FilmId]) REFERENCES [dbo].[Films] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Screenings] WITH CHECK ADD CONSTRAINT [FK_Screenings_Halls_HallId] 
FOREIGN KEY([HallId]) REFERENCES [dbo].[Halls] ([Id]) ON DELETE CASCADE
GO

-- Таблица рядов в зале
CREATE TABLE [dbo].[HallRows](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [HallId] [tinyint] NOT NULL,
    [Number] [smallint] NOT NULL,
    [Capacity] [smallint] NOT NULL,
    CONSTRAINT [PK_HallRows] PRIMARY KEY CLUSTERED ([Id] ASC)
)

ALTER TABLE [dbo].[HallRows] WITH CHECK ADD CONSTRAINT [FK_HallRows_Halls_HallId] 
FOREIGN KEY([HallId]) REFERENCES [dbo].[Halls] ([Id]) ON DELETE CASCADE
GO

-- Таблица билетов
CREATE TABLE [dbo].[Tickets](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [Row] [tinyint] NOT NULL,
    [Seat] [tinyint] NOT NULL,
    [Cost] [decimal](18, 2) NOT NULL,
    [ScreeningId] [bigint] NOT NULL,
    CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED ([Id] ASC)
)

ALTER TABLE [dbo].[Tickets] WITH CHECK ADD CONSTRAINT [FK_Tickets_Screenings_ScreeningId] 
FOREIGN KEY([ScreeningId]) REFERENCES [dbo].[Screenings] ([Id]) ON DELETE CASCADE
GO
`;

    return (
        <div className="database-scheme">
            <Card className="border-white mt-0">
                <Card.Body>
                    <Card.Text>
                        База данных представляет кинотеатр и содержит следующие таблицы:
                    </Card.Text>
                    
                    <div className="mb-4">
                        <SyntaxHighlighter 
                            language="sql" 
                            style={atomOneDark}
                            customStyle={{ 
                                borderRadius: '8px',
                                fontSize: '0.9rem',
                                padding: '1.5rem'
                            }}
                            showLineNumbers
                        >
                            {sqlCode}
                        </SyntaxHighlighter>
                    </div>

                    <Card.Text className="mt-4">
                        <Alert variant="info" className="mb-3">
                            Таблицы заполнены случайными данными. В каждой таблице по 100 000 записей.
                            Это сделано для оценки производительности вашего запроса
                        </Alert>
                    </Card.Text>
                </Card.Body>
            </Card>
        </div>
    );
}