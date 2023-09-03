const APPLICATION_URL = "https://localhost:4308/api";
const TODAY_DATE = new Date("2022-09-01");

const yesterdayBtn = document.getElementById('yesterday');
const todayBtn = document.getElementById('today');
const tomorrowBtn = document.getElementById('tomorrow');
const selectDateField = document.getElementById('select-date');

const authState = document.getElementById('authState');
const scheduleBody = document.getElementById('scheduleBody');
const currentDate = document.getElementById('currentDate');

yesterdayBtn.addEventListener("click", () => updateScheduleTable('yesterday'));
todayBtn.addEventListener("click", () => updateScheduleTable('today'));
tomorrowBtn.addEventListener("click", () => updateScheduleTable('tomorrow'));
selectDateField.addEventListener("change", (eventArgs) => updateScheduleTable(eventArgs));

authenticateUser();
updateScheduleTable('today');