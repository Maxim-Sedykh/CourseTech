import filmTableImage from '../../assets/images/filmTable.jpg';
import hallRowTableImage from '../../assets/images/hallRowTable.jpg';
import hallTableImage from '../../assets/images/hallTable.jpg';
import screeningTableImage from '../../assets/images/screeningTable.jpg';
import ticketTableImage from '../../assets/images/ticketTable.jpg';

export function FilmDbScheme() {
    return (
    <>
        <p>
            Допустим у нас база данных - Cinema. Которая основана на предметной области - кинотеатр (вы уже к ней подключились, поэтому писать
            такие операторы, как USE, GO не надо). И в этой базе есть следующие таблицы:
        </p>
        <b>
            CREATE TABLE [dbo].[Films](<br />
            [Id] [bigint] IDENTITY(1,1) NOT NULL,<br />
            [Name] [nvarchar](50) NOT NULL,<br />
            [Description] [nvarchar](200) NULL,<br />
            CONSTRAINT [PK_Films] PRIMARY KEY CLUSTERED<br />
            (<br />
            [Id] ASC<br />
            )<br />
            )<br />
            <br />
            CREATE TABLE [dbo].[Halls](<br />
            [Id] [tinyint] IDENTITY(1,1) NOT NULL,<br />
            [Name] [nvarchar](50) NOT NULL,<br />
            CONSTRAINT [PK_Halls] PRIMARY KEY CLUSTERED<br />
            (<br />
            [Id] ASC<br />
            )<br />
            )<br />
            <br />
            CREATE TABLE [dbo].[Screenings](<br />
            [Id] [bigint] IDENTITY(1,1) NOT NULL,<br />
            [HallId] [tinyint] NOT NULL,<br />
            [FilmId] [bigint] NOT NULL,<br />
            [Time] [datetime2](7) NOT NULL,<br />
            CONSTRAINT [PK_Screenings] PRIMARY KEY CLUSTERED<br />
            (<br />
            [Id] ASC<br />
            )<br />
            )<br />
            <br />
            ALTER TABLE [dbo].[Screenings]  WITH CHECK ADD  CONSTRAINT [FK_Screenings_Films_FilmId] FOREIGN KEY([FilmId])<br />
            REFERENCES [dbo].[Films] ([Id])<br />
            ON DELETE CASCADE<br />
            GO<br />
            <br />
            ALTER TABLE [dbo].[Screenings]  WITH CHECK ADD  CONSTRAINT [FK_Screenings_Halls_HallId] FOREIGN KEY([HallId])<br />
            REFERENCES [dbo].[Halls] ([Id])<br />
            ON DELETE CASCADE<br />
            GO<br />
            <br />
            CREATE TABLE [dbo].[HallRows](<br />
            [Id] [bigint] IDENTITY(1,1) NOT NULL,<br />
            [HallId] [tinyint] NOT NULL,<br />
            [Number] [smallint] NOT NULL,<br />
            [Capacity] [smallint] NOT NULL,<br />
            CONSTRAINT [PK_HallRows] PRIMARY KEY CLUSTERED<br />
            (<br />
            [Id] ASC<br />
            )<br />
            )<br />
            <br />
            ALTER TABLE [dbo].[HallRows]  WITH CHECK ADD  CONSTRAINT [FK_HallRows_Halls_HallId] FOREIGN KEY([HallId])<br />
            REFERENCES [dbo].[Halls] ([Id])<br />
            ON DELETE CASCADE<br />
            GO<br />
            <br />
            CREATE TABLE [dbo].[Tickets](<br />
            [Id] [bigint] IDENTITY(1,1) NOT NULL,<br />
            [Row] [tinyint] NOT NULL,<br />
            [Seat] [tinyint] NOT NULL,<br />
            [Cost] [decimal](18, 2) NOT NULL,<br />
            [ScreeningId] [bigint] NOT NULL,<br />
            CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED<br />
            (<br />
            [Id] ASC<br />
            )<br />
            )<br />
            <br />
            ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Screenings_ScreeningId] FOREIGN KEY([ScreeningId])<br />
            REFERENCES [dbo].[Screenings] ([Id])<br />
            ON DELETE CASCADE<br />
            GO<br />
            <br />
        </b>
        <p>
            Со следующими записями:<br /><br />
            Films<br /><br />
            <img className="img-fluid" src={filmTableImage} />
            <br /><br />
            HallRows<br /><br />
            <img className="img-fluid" src={hallRowTableImage} />
            <br /><br />
            Halls<br /><br />
            <img className="img-fluid" src={hallTableImage} />
            <br /><br />
            Screenings<br /><br />
            <img className="img-fluid" src={screeningTableImage} />
            <br /><br />
            Tickets<br /><br />
            <img className="img-fluid" src={ticketTableImage} />
            <br /><br />
        </p>
    </>)
}