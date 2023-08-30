var APPLICATION_URL = "https://localhost:4308";

const yesterdayBtn = document.getElementById('yesterday');
const todayBtn = document.getElementById('today');
const tomorrowBtn = document.getElementById('tomorrow');
const selectDateField = document.getElementById('select-date');

yesterdayBtn.addEventListener("click", () => changeSchedule('yesterday'));
todayBtn.addEventListener("click", () => changeSchedule('today'));
tomorrowBtn.addEventListener("click", () => changeSchedule('tomorrow'));
selectDateField.addEventListener("change", (e) => changeSchedule(e));

changeSchedule('today');

function getDateById(id) {
    const todayDate = new Date("2022-09-01"); // new Date();
    switch (id)
    {
        case 'yesterday':
            return addDays(todayDate, -1);
        case 'today':
            return todayDate;
        case 'tomorrow':
            return addDays(todayDate, +1);
        default:
            return new Date(selectDateField.value);
    }
}

async function changeSchedule(id) {
    const actualDateTime = getDateById(id);

    let actualDate = Intl.DateTimeFormat("uk-UA").format(actualDateTime);

    const dateItem = document.getElementById('currentDate');
    dateItem.textContent = actualDate;
    selectDateField.value = actualDateTime.toISOString().slice(0, 10);

    const response = await (await fetch(`${APPLICATION_URL}/api/schedule?date=${actualDate}`)).json();
    const scheduleBody = document.getElementById('scheduleBody');
    scheduleBody.textContent = '';
    scheduleBody.appendChild(createTable(response));
}

function createTable(lessons) {
    const fragment = document.createDocumentFragment();
    if (!lessons.length) {
        fragment.appendChild(scheduleNotFound());
    }
    lessons.forEach((lesson) => {
        const item = document.createElement("tr");
        const itemTime = `${lesson.start.slice(0, 5)}-${lesson.end.slice(0, 5)}`;
        let itemTeacher = lesson.teacher;

        if (itemTeacher.length) {
            itemTeacher = itemTeacher.split(" ");
            itemTeacher = `${itemTeacher[0]} ${itemTeacher[1][0]}. ${itemTeacher[2][0]}.`;
        }
        
        item.innerHTML = `                                
        <td>${itemTime}</td>
        <td>${lesson.discipline}</td>
        <td>${lesson.studyType}</td>
        <td>${lesson.cabinet}</td>
        <td><abbr style="border: none; text-decoration: none;" title="${lesson.teacher}">${itemTeacher}</abbr></td>`;
        fragment.appendChild(item);
    });
    return fragment;
}

function scheduleNotFound() {
    const item = document.createElement("tr");
    item.innerHTML = `<td colspan="5"><em>Розкладу на даний перiод часу не знайдено.</em></td>`
    return item;
}

function addDays(thisDate, days) {
    var date = new Date(thisDate);
    date.setDate(date.getDate() + days);
    return date;
}