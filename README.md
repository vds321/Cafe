# Программа для курсовой работы по Базам данных ЧелГУ ИИТ

## Тема курсовой работы:
Разработка базы данных для предметной области «Кафе»

### Провести анализ предметной области по следующему описанию:
Необходимо разработать систему для кафе, чтобы автоматизировать учет приготовленных блюд, базы ингредиентов, необходимых для их приготовления.
В добавок ко всему кафе необходимо фиксировать все заказы клиентов.

### Перечень входных (первичных) документов.
В качестве первичных документов для решения данной задачи используются :
|Блюдо|Вид|Вес|Продукт|Остаток|Дата приготовления|Время|Номер заказа|Кол-во порций|Цена|
|-----|---|---|-------|-------|------------------|-----|------------|-------------|----|
|борщ|суп|140|свекла|34|23.02.2016|13:07|1|1|68|
|борщ|суп|140|лук|26|23.02.2016|13:07|1|1|68|
|картофельное пюре|второе|150|картошка|65|03.02.2016|15:34|3|2|47|
|картофельное пюре|второе|150|масло|11|03.02.2016|15:34|3|2|47|


### Ограничения предметной области:

* Одно блюдо может быть приготовлено из нескольких ингредиентов;
* Вес блюда хранится в граммах;
* Существует несколько видов блюд: первое, второе, десерт, напитки;
* Остаток на складе фиксируется в килограммах;
* Дата и время хранится в стандартном формате (DD.MM.YYYY 15:30:00);
* У разных блюд разная цена;
* В одном заказе может быть не более 6 наименований блюд, а количество каждого наименования не более 2-х штук.

1. Создать базу данных в выбранной СУБД с учетом ограничений предметной области.
2. Реализовать следующие отчеты (запросы):
* Найти блюда, которые содержат все ингредиенты из указанных пользователем при поиске;
* Вывести всю информацию о тех заказах, где был заказан хотя бы один десерт за период времени, указанный пользователем. 
* Вывести все продукты, которые входят в состав первых блюд.
* Вывести информацию о блюдах, цена которой выше средней по меню и при этом эти блюда не были заказаны ни разу за период времени, указанный пользователем.
3. Выбрать язык программирования и разработать приложение для работы с БД (формы ввода/редактирования данных и отчеты).
