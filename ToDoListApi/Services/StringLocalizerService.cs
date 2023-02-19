using Microsoft.Extensions.Localization;
using System.Globalization;

namespace ToDoListApi.Services
{
    public class StringLocalizerService : IStringLocalizer
    {
        private readonly Dictionary<string, Dictionary<string, string>> _localizedStrings;
        public StringLocalizerService()
        {            
            _localizedStrings = new Dictionary<string, Dictionary<string, string>>
            {
                {
                    "en-US",
                    new Dictionary<string, string>
                    {
                        { "ExistsTask", "Task with same id already exists." },
                        { "NotFoundTask", "Task with same id not found." },
                        { "AddTask", "Task added successfully." },
                        { "DeleteTask", "Task deleted successfully." },
                        { "EmptyTodoList", "To Do list is empty now. Please add task." },
                        { "UpdateTask", "Task updated successfully." },
                        { "Unauthorized", "You did not authorize. Please authorize!" }
                    }
                },
                {
                    "uk-UA",
                    new Dictionary<string, string>
                    {
                        { "ExistsTask", "Завдання з таким номером вже існує." },
                        { "NotFoundTask", "Завдання з таким номером не знайдено." },
                        { "AddTask", "Завдання успішно додано." },
                        { "DeleteTask", "Завдання успішно видалено." },
                        { "EmptyTodoList", "Список завдань зараз порожній. Будь ласка, додайте завдання." },
                        { "UpdateTask", "Завдання успішно оновлено." },
                        { "Unauthorized", "Ви не авторизовані. Будь ласка, авторизуйтесь!" }
                    }
                }
            };
        }
        public LocalizedString this[string name]
        {
            get
            {
                return this[name, name];
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var culture = CultureInfo.CurrentCulture.Name;
                if (_localizedStrings.TryGetValue(culture, out var localizedStrings))
                {
                    if (localizedStrings.TryGetValue(name, out var localizedString))
                    {
                        return new LocalizedString(name, localizedString);
                    }
                }
                return new LocalizedString(name, name);
            }
        }
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var culture = CultureInfo.CurrentCulture.Name;
            var localizedStrings = new List<LocalizedString>();
            do
            {
                if (_localizedStrings.TryGetValue(culture, out var strings))
                {
                    foreach (var entry in strings)
                    {
                        localizedStrings.Add(new LocalizedString(entry.Key, entry.Value));
                    }
                }
                culture = CultureInfo.GetCultureInfo(culture).Parent.Name;
            } 
            while (includeParentCultures && culture != "");
            return localizedStrings;
        }
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new StringLocalizerService();
        }
    }
}
