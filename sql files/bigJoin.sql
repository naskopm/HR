
	SELECT 
    e.emp_id,
    e.FirstName,
    e.LastName,
    e.YearOfBirth,
	e.salary,
	ti.title,
    STRING_AGG(DISTINCT l.language, ', ') AS languages,
    STRING_AGG(DISTINCT devs.skill, ', ') AS skills,
    STRING_AGG(DISTINCT man.idSubordinate::text, ', ') AS subordinates,
    bon.bonus
FROM Employees e
LEFT JOIN languageSpoken ls ON e.emp_id = ls.emp_id
LEFT JOIN languages l ON ls.language_spoken = l.language_id
LEFT JOIN employeeTitles et ON e.emp_id = et.emp_id
LEFT JOIN titles ti ON et.emp_title = ti.title_id
LEFT JOIN developers dev ON e.emp_id = dev.dev_id
LEFT JOIN developerSkills devs ON dev.dev_skill = devs.id_skill
LEFT JOIN manager man ON e.emp_id = man.idManager
LEFT JOIN bonuses bon ON e.emp_id = bon.emp_id
GROUP BY e.emp_id, e.FirstName, e.LastName, e.YearOfBirth, bon.bonus, ti.title;