async function updateScheduleTable(dateTag) {
    const actualDateTime = getDateByDateTag(dateTag);
    const actualDate = Intl.DateTimeFormat("uk-UA").format(actualDateTime);
    
    // Update current date and calendar value
    currentDate.textContent = actualDate;
    selectDateField.value = actualDateTime.toISOString().slice(0, 10);

    // Update schedule table
    const response = await loadSchedule(actualDateTime);
    scheduleBody.textContent = '';
    scheduleBody.appendChild(createTableFragment(response));

    // Add electricity popovers
    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
    const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl, {
        trigger: 'focus'
      }))
}

function getDateByDateTag(id) {
    const todayDate = TODAY_DATE;
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

async function loadSchedule(date) {
    const actualDate = Intl.DateTimeFormat("uk-UA").format(date);
    return await (await fetch(`${APPLICATION_URL}/schedule?date=${actualDate}&provideOutages=true`)).json();
}

function createTableFragment(lessons) {
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

        let outage = "";
        let outages_info = "";
        
        if (lesson.outages.length)
        {
            outage = lesson.outages.some((x) => x.type == 2) ? "img/outage_definite.png" : "img/outage_possible.png";
            lesson.outages.forEach((item) => {
                outages_info += `<b>${item.start.slice(0, 5)}-${item.end.slice(0, 5)}</b> - ${item.text}<br>`;
            });
        }
        item.innerHTML = `
        <td><a tabindex="0" data-bs-toggle="popover" data-bs-html="true" data-bs-offset="[0, 20]" data-bs-placement="left" data-bs-trigger="focus" data-bs-title="Список вiдключень"  data-bs-content="${outages_info}"><input class="img-status" type="image" src="${outage}"/></a></td>
        <td>${itemTime}</td>
        <td>${lesson.discipline}</td>
        <td>${lesson.studyType}</td>
        <td>${lesson.cabinet}</td>
        <td><abbr style="text-decoration: none; border: none;" title="${lesson.teacher}">${itemTeacher}</abbr></td>`;
        fragment.appendChild(item);
    });
    return fragment;
}

function scheduleNotFound() {
    const item = document.createElement("tr");
    item.innerHTML = `<td colspan="10"><em>Розкладу на даний перiод часу не знайдено.</em></td>`
    return item;
}

function addDays(thisDate, days) {
    var date = new Date(thisDate);
    date.setDate(date.getDate() + days);
    return date;
}