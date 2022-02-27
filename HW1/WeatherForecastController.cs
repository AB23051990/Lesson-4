using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{   /// Напишите собственный контроллер и методы в нём, которые бы предоставляли следующую функциональность:
    /// a.Возможность сохранить температуру в указанное время.
    /// b.Возможность отредактировать показатель температуры в указанное время.
    /// c.Возможность удалить показатель температуры в указанный промежуток времени.
    /// d.Возможность прочитать список показателей температуры за указанный промежуток времени.

    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private int _TemperatureC;
        private DateTime _Date;
        public int TemperatureC1 { get => _TemperatureC; set => _TemperatureC = value; }
        public DateTime Date1 { get => _Date; set => _Date = value; }

        /*
        public WeatherForecastController(int TemperatureC)
        {_TemperatureC = TemperatureC;}
        */

        [HttpGet, Route("/")]
        public string Hello()
        {
            return $"Введите путь:" +
                   $"\nHttpPost:   [/save] - сохранить температуру в указанное время" +
                   $"\nHttpPatch:  [/update/'число'] - отредактировать показатель температуры в указанное время" +
                   $"\nHttpDelete: [/Delete] - удалить показатель температуры в указанный промежуток времени" +
                   $"\nHttpGet:    [/statistic] - прочитать список показателей температуры за указанный промежуток времени";
        }

        [HttpPost, Route("save")]
        public string Save()
        {
            var rng = new Random();
            _TemperatureC = rng.Next(-25, 55);
            _Date = DateTime.Now;
            return $"Сейчас: Температура-[{ _TemperatureC}℃] Время-[{_Date}]";
        }

        [HttpPatch, Route("update/{a}")]
        public string Update([FromRoute] int a)
        {
            _TemperatureC = a;
            return $"смена температуры на [{_TemperatureC}]℃ в [{_Date}]";
        }

        [HttpDelete, Route("Delete")]
        public string Delete()
        {
            _TemperatureC = 0;
            return $"смена температуры на [{_TemperatureC}]℃ в [{_Date}]";
        }

        [HttpGet, Route("statistic")]
        public string Statistic()
        {
            return $"текущее значение температуры [{_TemperatureC}]℃ в [{_Date}]";
        }
    }
}