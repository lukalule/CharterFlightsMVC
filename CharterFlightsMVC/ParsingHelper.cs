using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CharterFlightsMVC
{
    public class ParsingHelper
    {
        public static void ParseIataCodesHtml()
        {
            for (int i = 65; i < 91; i++)
            {

                char a = (char)i;

                using (StreamReader sr = File.OpenText($"IataCodesSorted/sorted{a}.txt"))
                using (StreamWriter sw = new StreamWriter($"IataCodes/final{a}.txt"))
                {
                    string str = String.Empty;
                    int brojReda = 1;
                    string iataAndLocation = String.Empty;
                    while ((str = sr.ReadLine()) != null)
                    {
                        if (str.Contains("colspan") || str.Contains("<th") || str.Contains("</th>") || str.Contains("sortbottom") || string.IsNullOrEmpty(str))
                            continue;
                        if (brojReda == 2 || brojReda == 5) // u drugom i petom redu <tr></tr> se nalaze iata code i mjesto polaska, respektivno
                        {
                            //<td>AAA</td>
                            if (brojReda == 2)
                                iataAndLocation = str.Substring(str.IndexOf('>') + 1, 3) + ",";
                            else
                            {//rwer<sup d>dd</sup>fsdfsd
                                if (str.Contains("</sup>"))
                                    str = str.Remove(str.IndexOf("<sup"), str.LastIndexOf("sup>") + 4 - str.IndexOf("<sup"));

                                while (str.Contains(","))
                                {
                                    int indexZadnjegSlova;
                                    if (str.ElementAt(str.IndexOf(",") - 1) == '>') //ako je naziv grada smjesten u <a> tag
                                        indexZadnjegSlova = str.IndexOf(",") - 5;
                                    else
                                        indexZadnjegSlova = str.IndexOf(",") - 1;

                                    int indexZatvoreneUglasteZagrade = indexZadnjegSlova;
                                    while (str.ElementAt(indexZatvoreneUglasteZagrade--) != '>') ;

                                    indexZatvoreneUglasteZagrade++;
                                    iataAndLocation += str.Substring(indexZatvoreneUglasteZagrade + 1, indexZadnjegSlova - indexZatvoreneUglasteZagrade) + ", "; // dodaje naziv grada

                                    int indexZadnjegZareza = str.IndexOf(',');
                                    str = str.Remove(str.IndexOf(','), 1);
                                    if (!str.Contains(","))
                                    {
                                        str = str.Insert(indexZadnjegZareza, ",");
                                        break;
                                    }
                                }

                                //kupi naziv drzave kad se naziv nalazi u <a> tagu
                                if (str.ElementAt(str.LastIndexOf("</td>") - 1) == '>')
                                {
                                    int indexZadnjegSlova = str.LastIndexOf("</td>") - 5;

                                    int indexZatvoreneUglasteZagrade = indexZadnjegSlova;
                                    while (str.ElementAt(indexZatvoreneUglasteZagrade--) != '>') ;

                                    indexZatvoreneUglasteZagrade++;
                                    iataAndLocation += str.Substring(indexZatvoreneUglasteZagrade + 1, indexZadnjegSlova - indexZatvoreneUglasteZagrade);
                                }
                                else //kad je obicni tekst
                                {
                                    int indexZadnjegSlova = str.LastIndexOf("</td>") - 1;

                                    int indexZadnjegZareza = indexZadnjegSlova;
                                    while (str.ElementAt(indexZadnjegZareza--) != ',') ; //tu pocinje naziv drzave

                                    indexZadnjegZareza += 2;
                                    iataAndLocation += str.Substring(indexZadnjegZareza + 1, indexZadnjegSlova - indexZadnjegZareza);
                                }
                                //\"Mala Mala, Mala Mala, South Africa npr. (ostane u slucaju kad naziv grada ili drzave sadrzi zarez)
                                if (iataAndLocation.Contains("<a href"))
                                {
                                    int brisiOd = iataAndLocation.IndexOf("<a");
                                    int indexZadnjegNavodnika = iataAndLocation.LastIndexOf('"') + 1;
                                    int indexZareza = indexZadnjegNavodnika;
                                    while (iataAndLocation.ElementAt(indexZareza++) != ',') ;
                                    iataAndLocation = iataAndLocation.Remove(brisiOd, indexZareza - brisiOd);
                                }
                                sw.WriteLine(iataAndLocation);
                                iataAndLocation = String.Empty;
                            }
                            brojReda++;
                            continue;
                        }
                        brojReda++;
                        if (i == 78 && brojReda == 9)
                            brojReda = 1;
                        else if (i > 78 && brojReda == 7)
                            brojReda = 1;
                    }
                }
            }
        }

        public void CreateJsonArrayFileForSelect2()
        {
            JArray codes = new JArray();
            int j = 1;
            using (StreamWriter file = System.IO.File.CreateText("IataCodes/AllCodes.json"))
            {
                for (int i = 65; i < 91; i++)
                {
                    char a = (char)i;
                    string str = String.Empty;
                    using (StreamReader sr = System.IO.File.OpenText($"IataCodes/{a}.txt"))
                    {
                        while ((str = sr.ReadLine()) != null)
                        {
                            codes.Add(new JObject()
                            {
                                new JProperty("id", j++),
                                new JProperty("text", str)
                            });
                        }
                    }
                }
                using (JsonTextWriter writer = new JsonTextWriter(file))
                {
                    codes.WriteTo(writer);
                }
            }

        }
    }
}
