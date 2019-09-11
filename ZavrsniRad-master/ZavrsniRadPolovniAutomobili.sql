Create database PolovniAutomobili
collate Serbian_Latin_100_CI_AI
go
Use PolovniAutomobili
go

create table Marka(
MarkaId int identity(1,1) primary key not null,
Naziv nvarchar(50) not null
);
--Marke
insert into Marka values('Alfa Romeo');
insert into Marka values('Audi');
insert into Marka values('BMW');
insert into Marka values('Citroen');
insert into Marka values('Fiat');
--
create table Modeli(
ModelId int identity(1,1) primary key not null,
MarkaId int foreign key references Marka(MarkaId) not null,
Naziv nvarchar(50) not null
);
--Modeli
--AlfaRomeo
insert into Modeli values(1,'145');
insert into Modeli values(1,'146');
insert into Modeli values(1,'147');
--Audi
insert into Modeli values(2,'A1');
insert into Modeli values(2,'A2');
insert into Modeli values(2,'A3');
--BMW
insert into Modeli values(3,'X1');
insert into Modeli values(3,'X3');
insert into Modeli values(3,'X5');
--Citroen
insert into Modeli values(4,'C1');
insert into Modeli values(4,'C2');
insert into Modeli values(4,'C3');
--Fiat
insert into Modeli values(5,'Bravo');
insert into Modeli values(5,'Stilo');
insert into Modeli values(5,'Grande Punto');
----
create table TipVozila(
TipVozilaId int identity(1,1) primary key not null,
Naziv nvarchar(50) not null
);
--Tipvozila
insert into TipVozila values('Coupe');
insert into TipVozila values('Hecbek');
insert into TipVozila values('Kabrio');
insert into TipVozila values('Karavan');
insert into TipVozila values('Limuzina');
----
create table Vozilo(
VoziloId int identity(1,1) primary key not null,
MarkaId int foreign key references Marka(MarkaId) not null,
ModelId int foreign key references Modeli(ModelId) not null,
TipVozilaId int foreign key references TipVozila(TipVozilaId) not null,
KorisnikId nvarchar(450) not null,
Kubikaza nvarchar(20) not null,
Snaga nvarchar(20) not null,
Kilometraza char(6) not null,
Pogon nvarchar(20) not null,
Menjac nvarchar(20) not null,
BrojBrzina char(1) null,
Cena int not null,
Slika varbinary(max) null,
SlikaTip nvarchar(20) null,
Opis nvarchar(200) null
);
------
create table Komentar(
KomentarId int identity(1,1) primary key not null,
VoziloId int foreign key references Vozilo(VoziloId) not null,
KorisnikId nvarchar(450) not null,
Korisnik nvarchar(20) not null,
Opis nvarchar(300) not null
);