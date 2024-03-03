﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wtt_main_server_data.Database.Abstract;
using wtt_main_server_data.Enums;

namespace wtt_main_server_data.Database.TestScenarios;

#pragma warning disable CS8618

public class DbImapAction : ADbAction
{
	public override string Type => "Imap";

	/* Аккаунт IMAP, которые будет использован для подтягивания письма.
	 */
	public Guid UserImapAccountGuid { get; set; }

	/* Нижепредставленные поля нужны для первого режима работы
	 * программы - поиск по регексу. Нужное письмо определяется
	 * через соответствие по логическому И, т.е. если и 
	 * тема и отправитель и тело соответствуют, то письмо
	 * трактуется как подходящее. Далее ещё один регекс
	 * используется уже для вытаскивания значения.
	 * А также даётся возможно ещё и написать скрипт, но
	 * скорее всего сейчас (на начальном этапе разработки
	 * системы) это не будет сразу доступно, но как возможность
	 * я лучше заложу это сейчас.
	 */
	public string? SubjectRegex { get; set; }
	public string? SenderRegex { get; set; }
	public string? BodyRegex { get; set; }
	public string? BodySearchRegex { get; set; }
	public string? BodyProcessingScript { get; set; }

	/* Второй, более простой режим работы. Опять же, покрывает
	 * просто большую часть простейших ситуаций. Позволяет найти
	 * в теле письма нужную часть и далее сохранить её в переменную.
	 * У меня нет желания тут писать очень сложную систему, поэтому
	 * тело письма будет просто сплитом разбиваться.
	 * 
	 * UPD: принято решение исключить из проекта.
	 * Вместо данного енама следует предлогать юзеру
	 * автоматически заполнить регекс-выражение, что
	 * для поиска номера будет соответственно представлять
	 * строку "\d+", а для поиска ссылки "https://.+/.+",
	 */
	//public EmailAutoparsingMode AutoparsingMode { get; set; }

	public int MinSearchLength { get; set; }
	public int MaxSearchLength { get; set; }
	public string[] SearchMustContain { get; set; }
}
