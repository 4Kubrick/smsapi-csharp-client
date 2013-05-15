csharp-client
===========

Klient napisany w j�zyku C#, pozwalaj�cy na wysy�anie wiadomo�ci SMS, MMS, VMS oraz zarz�dzanie kontem w serwisie SMSAPI.pl

Przyk�ad wysy�ki sms:
```c#
try
{
	var smsApi = new SMSApi.Api.SMSFactory(client());


	var result =
		smsApi.ActionSend()
			.SetText("test message")
			.SetTo("694562829")
			.SetDateSent(DateTime.Now.AddHours(2))
			.Execute();

	System.Console.WriteLine("Send: " + result.Count);

	string[] ids = new string[result.Count];

	for (int i = 0, l = 0; i < result.List.Count; i++)
	{
		if (!result.List[i].isError())
		{
			if (!result.List[i].isFinal())
			{
				ids[l] = result.List[i].ID;
				l++;
			}
		}
	}

	System.Console.WriteLine("Get:");
	result =
		smsApi.ActionGet()
			.Ids(ids)
			.Execute();

	foreach (var status in result.List)
	{
		System.Console.WriteLine("ID: " + status.ID + " NUmber: " + status.Number + " Points:" + status.Points + " Status:" + status.Status + " IDx: " + status.IDx);
	}

	var deleted =
		smsApi
			.ActionDelete()
				.Id(ids)
				.Execute();

	System.Console.WriteLine("Deleted: " + deleted.Count);
}
catch (SMSApi.Api.ActionException e)
{
	/**
	 * B��dy zwi�zane z akcj� (z wy��czeniem b��d�w 101,102,103,105,110,1000,1001 i 8,666,999,201)
	 * http://www.smsapi.pl/sms-api/kody-bledow
	 */
	System.Console.WriteLine(e.Message);
}
catch (SMSApi.Api.ClientException e)
{
	/**
	 * 101 Niepoprawne lub brak danych autoryzacji.
	 * 102 Nieprawid�owy login lub has�o
	 * 103 Brak punk�w dla tego u�ytkownika
	 * 105 B��dny adres IP
	 * 110 Us�uga nie jest dost�pna na danym koncie
	 * 1000 Akcja dost�pna tylko dla u�ytkownika g��wnego
	 * 1001 Nieprawid�owa akcja
	 */
	System.Console.WriteLine(e.Message);
}
catch (SMSApi.Api.HostException e)
{
	/* b��d po stronie servera lub problem z parsowaniem danych
	 * 
	 * 8 - B��d w odwo�aniu
	 * 666 - Wewn�trzny b��d systemu
	 * 999 - Wewn�trzny b��d systemu
	 * 201 - Wewn�trzny b��d systemu
	 * SMSApi.Api.HostException.E_JSON_DECODE - problem z parsowaniem danych
	 */
	System.Console.WriteLine(e.Message);
}
catch (SMSApi.Api.ProxyException e)
{
	// b��d w komunikacji pomiedzy klientem i serverem
	System.Console.WriteLine(e.Message);
}
```
