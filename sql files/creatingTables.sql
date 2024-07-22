create table Employees(
	emp_id
	serial primary key,
	FirstName varchar(100),
	LastName Varchar(100),
	YearOfBirth date,
);
drop table languageSpoken
CREATE TABLE languageSpoken (
    emp_id INT,
    language_spoken INT,
    PRIMARY KEY (emp_id, language_spoken),
    FOREIGN KEY (emp_id) REFERENCES Employees(emp_id),
	FOREIGN KEY (language_spoken) REFERENCES languages(language_id)
);
Create table languages(
language_id serial primary key,
language varchar(30)
);
Create table titles(
	title_id serial primary key,
	title varchar(100)
);
create table manager(
	idManager INT,
	idSubordinate INT,
	Foreign key (idManager) references Employees(emp_id),
	Foreign key (idSubordinate) references Employees(emp_id),
	primary key (idManager, idSubordinate)
);
create table developerSkills(
	id_skill serial primary key,
	skill varchar(30)
	);
create table developers(
	dev_id INT,
	dev_skill int,
	foreign key (dev_id) references employees(emp_id),
	foreign key (dev_skill) references developerSkills(id_skill),
	primary key(dev_id, dev_skill)
	);
create table bonuses(
	emp_id INT,
	bonus float,
	foreign key(emp_id) references employees(emp_id),
	primary key(emp_id)
	);
create table employeeTitles(
	emp_id INT,
	emp_title INT,
	foreign key (emp_id) references employees(emp_id),
	foreign key (emp_title) references titles(title_id),
	primary key(emp_id)
);