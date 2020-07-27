using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Windows;
using Map.Model;

namespace Map.ViewModel
{
    public class MapViewModel : ViewModel
    {
        
        MapModel Map = new MapModel();

        public ObservableCollection<String> AllItems  //Биндинг истории поиска
        {
            get { return Map.AllItems; }
            
        }
        public string CurrentItem //Биндинг искомого элемента
        {
            get { return Map.CurrentItem; }
            set
            {
                if (Map.CurrentItem == value) return;
                Map.CurrentItem = value;
                OnPropertyChange();
            }
        }
        public Location Location //Биндинг точки на карте
        {
            get { return Map.Pushpin.Location; }
        }

        public string Error //Биндинг текста ошибки
        {
            get { return Map.Error; }
        }
        public string PushpinVisibility //Биндинг видимости точки на карте
        {
            get
            {
                if (Map.Pushpin.Visibility == Visibility.Visible)
                { 
                    return "Visible";
                } else 
                {
                    return "Collapsed";
                };
            }
        }


        public ICommand EnterPointCommand { get; set; } 

        public MapViewModel()
        {
            EnterPointCommand = new Command(EnterPointMethod, canExecuteMethod);

            Map.AllItems = new ObservableCollection<string>();

            Map.Pushpin.Visibility = Visibility.Collapsed; //Изначально точка невидима

            OnPropertyChange();

        }
        private void EnterPointMethod(object Parameters) {

          if ((Map.CurrentItem != "")&&(Map.CurrentItem != null)) //Если строка поиска не пуста
            {
                Map.Pushpin.Location = Map.GetLocation(Map.CurrentItem); //Поиск локации по запросу

                if (Map.Pushpin.Location != null) //Если локация надена
                {
                    if (Map.AllItems.IndexOf(Map.CurrentItem) == -1) //Если адреса нет в списке историй
                    {
                        Map.AllItems.Add(Map.CurrentItem); //Добавление адреса в историю
                    }
                    Map.Pushpin.Visibility = Visibility.Visible; 
                    Map.Error = ""; //Очистка TextBox ошибки
                }
                else //Если локация не найдена
                {
                    Map.Pushpin.Visibility = Visibility.Collapsed; 
                    Map.Error = "Адрес не найден";
                }
            }
            else //Если строка поиска пуста
            {
                Map.Error = "Введите корректный адрес";
            }


            Map.CurrentItem = ""; //очистка строки поиска
            OnPropertyChange();

        }


    }
}

