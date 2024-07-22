delete from employeetitles et
where et.emp_id = 4;
do $$
declare 
	title integer;
begin
with joined as(
	select t.title_id,t.title,ts.emp_id
	from titles t
	left join employeetitles ts on ts.emp_title = t.title_id 
),
alsoJoined as(
select j.title_id
from joined j
where j.title = 'Analyst')
select alsoJoined.title_id from alsoJoined  limit 1
into title;
insert into employeetitles(emp_id, emp_title)
values(4,title);
end $$;
select * from titles
into language_id
insert into languagespoken(emp_id,language_spoken)
select joined.language_id, joined.emp_id from joined
values(joined.emp_id, joined.language_id)









with joined as(
	select t.title_id,t.title,ts.emp_id
	from titles t
	left join employeetitles ts on ts.emp_title = t.title_id 
),
alsoJoined as(
select j.title_id, j.emp_id
from joined j
where j.title = 'Developer')
select * from alsoJoined