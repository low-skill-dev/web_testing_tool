﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Models.Enums;
using Models.Structures;
using CommonLibrary.Models;
using Reinforced.Typings.Attributes;

namespace Models.Database.TestScenarios;

#pragma warning disable CS8618

/* ==========================================================================
 * 
 * Идея состоит в раздельном хранении сценария тестирования и его инстансов,
 * проще говоря, его запусков. Сам по-себе сценарий должен иметь возможность
 * задержки (специальный тип действия), поэтому не получится просто грузить
 * работающие сценарии в раму.
 * 
 * ==========================================================================
 * 
 * Предлагается следующая схема:
 * 
 * Пользователь визуально накидывает сценарии тестирования. В базу сохраняется
 * сценарий, которому записывается название, цена пресета (особоая единица,
 * означающая стоимость исполнения запросов для сервера), пользователя,
 * создавшего пресет, uuid, прочие служебные поля.
 * 
 * Но... Для каждого типа действия в БД служит отдельная таблица, т.к. каждое
 * из них имеет совершенно разные поля. Следственно помимо массива айдишников
 * действий у каждого сценария, для каждого из них еще и нужно указать, в какую
 * таблицу смотреть, проще говоря, какой тип имеет действие. Поскольку EF не
 * поддерживает напрямую кортежи, мы будем использовать два массива.
 * 
 * ==========================================================================
 */
//[TsClass(IncludeNamespace = false, Order = 500)]
public class DbTestScenario : ObjectWithUser
{
	public string Name { get; set; } = "";

	/* Для отправки почты следует сделать отдельное действие,
	 * а эта настройка должна позволять глобально отключить
	 * отправку почты по всему сценарию.
	 */
	public bool EnableEmailNotifications { get; set; } = false;

	/* Есть необходимость хранить как тип действия, так
	 * и его номер, поскольку разные по типу действия будут
	 * лежать в разных таблицах. В данном случае идеально
	 * бы подошел кортеж, но NpgSQL их не поддерживает.
	 * Поэтому решено использовать два массива, поверх которых
	 * можно будет реализовать абстрагирующий метод.
	 * 
	 * upd. Соответствует полю ADbAction.Type
	 * 
	 * upd. Да пошло оно нахуй. Такой хуйней я ещё не занимался.
	 *		Ставлю jsonb и пусть идут нахуй со своим SQL.
	 *		А когда я добавлю 5 новых типов действий мне
	 *		что, ещё 5 таблиц добавлять? И везде это
	 *		прописывать???
	 */
	[Column(TypeName = "jsonb")]
	public ActionsCollection ActionsJson { get; set; }
	public Guid EntryPoint { get; set; } = Guid.Empty;


	/* Киллер-фича - параметры в сценариях, реализуется
	 * аналогичным образом. Сами параметры будут указываться
	 * в ещё одной отдельной таблице RunArgCollection (или
	 * типа того), которая позволит собственно их сохранять
	 * на будущее и переиспользовать.
	 * 
	 * ЗДЕСЬ УКАЗАНО ТО, ЧТО СЦЕНАРИЙ МОЖЕТ ПРИНЯТЬ,
	 * А КОНКРЕТНЫЙ НАБОР ПАРАМЕТРО ЗАДАЕТСЯ ОТДЕЛЬНО
	 */
	 [Obsolete("Отказ от типов, только голые аргументы")]
	public TestScenarioArgTypes[] ArgTypes { get; set; } = [];
	public string[] ArgNames { get; set; } = [];

	//public void EncodeActions(ActionsCollection actions)
	//{
	//	this.ActionsJson = System.Text.Json.JsonSerializer.Serialize(actions);
	//}

	public int? RunIntervalMinutes { get; set; }
	public TimeOnly[]? RunTimes { get; set; }
}
