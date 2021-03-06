using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace MorseCodeFlashlightApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //InitializeComponent();
            Label title = new Label
            {
                Text = "Добро пожаловать в мое приложение для фонарика!",
                FontSize = 32
            };
            Button btn = new Button
            {
                Text = "Начать!"
            };
            btn.Clicked += Btn_Clicked;
            Content = new StackLayout
            {
                Children = { title, btn }
            };
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FlashlightPage());
        }
    }
    public class FlashlightPage : ContentPage
    {
        bool on, flick, busy;
        Label textLabel;
        Slider slider;
        public FlashlightPage()
        {
            Button _switch = new Button
            {
                Text = "Включить фонарик!"
            };
            slider = new Slider
            {
                Maximum = 1000.0,
                Minimum = 1.0,
            };
            Button enableSlider = new Button
            {
                Text = "Включить мерцание."
            };
            Button enableSos = new Button
            {
                Text = "Включить SOS сигнал."
            };
            Button morseTranslate = new Button
            {
                Text = "Создать новое сообщение по морзе."
            };
            Button chooseAndPlay = new Button
            {
                Text = "Выбрать сообщение из БД и сыграть."
            };
            Button stopPlaying = new Button
            {
                Text = "Остановить любое проигрывание."
            };
            Button chooseAndDelete = new Button
            {
                Text = "Выбрать и удалить сообщение."
            };
            Button chooseAndWatch = new Button
            {
                Text = "Выбрать и просмотреть сообщение."
            };
            textLabel = new Label
            {
                FontSize = 24,
                Text = " ",
                HorizontalOptions = LayoutOptions.Center,
            };
            chooseAndPlay.Clicked += ChooseAndPlay_Clicked;
            morseTranslate.Clicked += MorseTranslate_Clicked;
            enableSos.Clicked += EnableSos_Clicked;
            enableSlider.Clicked += EnableSlider_Clicked;
            chooseAndWatch.Clicked += ChooseAndWatch_Clicked;
            _switch.Clicked += _switch_Clicked;
            chooseAndDelete.Clicked += ChooseAndDelete_Clicked;
            stopPlaying.Clicked += StopPlaying_Clicked;
            Content = new StackLayout
            {
                Children = { _switch, slider, enableSlider, enableSos, morseTranslate, chooseAndPlay, chooseAndDelete, chooseAndWatch, stopPlaying, textLabel }
            };
        }

        private async void ChooseAndWatch_Clicked(object sender, EventArgs e)
        {
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            List<MorseCodeTemplate> items = (List<MorseCodeTemplate>)App.Database.GetItems();
            string[] names = new string[items.Count];
            for (int i = 0; i < names.Length; i++)
            {
                pairs.Add((int)items[i].ID, items[i].Name);
                names[i] = items[i].Name;
            }

            string action = await DisplayActionSheet("Выберите сообщение для просмотра.", "Отмена", null, names);
            if (action == "Отмена")
                return;
            int id = pairs.FirstOrDefault(x => x.Value == action).Key;
            await Navigation.PushAsync(new WatchPage(App.Database.GetItem(id)));
        }

        private async void ChooseAndDelete_Clicked(object sender, EventArgs e)
        {
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            List<MorseCodeTemplate> items = (List<MorseCodeTemplate>)App.Database.GetItems();
            string[] names = new string[items.Count];
            for (int i = 0; i < names.Length; i++)
            {
                pairs.Add((int)items[i].ID, items[i].Name);
                names[i] = items[i].Name;
            }

            string action = await DisplayActionSheet("Выберите сообщение для удаленя.", "Отмена", null, names);
            if (action == "Отмена")
                return;
            int id = pairs.FirstOrDefault(x => x.Value == action).Key;
            App.Database.DeleteItem(id);
        }

        private void StopPlaying_Clicked(object sender, EventArgs e)
        {
            busy = false;
        }

        private async void ChooseAndPlay_Clicked(object sender, EventArgs e)
        {
            if (busy)
                return;
            Dictionary<int, string> pairs = new Dictionary<int, string>();
            List<MorseCodeTemplate> items = (List<MorseCodeTemplate>)App.Database.GetItems();
            string[] names = new string[items.Count];
            for (int i = 0; i < names.Length; i++)
            {
                pairs.Add((int)items[i].ID, items[i].Name);
                names[i] = items[i].Name;
            }

            string action = await DisplayActionSheet("Выберите сообщение.","Отмена",null, names);
            if (action == "Отмена")
                return;
            int id = pairs.FirstOrDefault(x => x.Value == action).Key;
            string[] arr = JsonConvert.DeserializeObject<string[]>(App.Database.GetItem(id).Morse);
            playMorse(arr);
        }

        private async void playMorse(string[] morseCode)
        {
            textLabel.Text = "";
            busy = true;
            for (int i = 0; i < morseCode.Length; i++)
            {
                MorseCode.InitializeDictionary();
                string letter = MorseCode._morseRusDictionary.FirstOrDefault(x => x.Value == morseCode[i]).Key.ToString();
                if (letter == "")
                {
                    
                    MorseCode._morseAlphabetDictionary.FirstOrDefault(x => x.Value == morseCode[i]).Key.ToString();

                }
                textLabel.Text += letter;
                for (int j = 0; j < morseCode[i].Length; j++)
                {
                    if (!busy)
                        return;
                    if (morseCode[i][j] == '.')
                    {
                        await Flashlight.TurnOnAsync();
                        await Task.Delay(500);
                        await Flashlight.TurnOffAsync();
                        await Task.Delay(500);
                    }
                    else if (morseCode[i][j] == '-')
                    {
                        await Flashlight.TurnOnAsync();
                        await Task.Delay(1000);
                        await Flashlight.TurnOffAsync();
                        await Task.Delay(1000);
                    }
                    if(j == morseCode[i].Length - 1)
                    {
                        await Task.Delay(1500);
                    }
                }
            }
            busy = false;
        }

        private async void MorseTranslate_Clicked(object sender, EventArgs e)
        {   
            await Navigation.PushAsync(new PopUpMorse());
        }

        private async void EnableSos_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (on)
            {
                busy = !busy;
                if (busy)
                {
                    btn.Text = "Выключить SOS.";
                    while(busy && on)
                    {
                        playMorse(new string[] { "...", "---", "..." });
                    }
                }
                else
                {
                    btn.Text = "Включить SOS сигнал.";
                }

            }
        }

        private async void EnableSlider_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (on)
            {
                flick = !flick;
                if (flick)
                {
                    btn.Text = "Выключить мерцание.";
                    await FLICK();
                }
                else
                {
                    btn.Text = "Включить мерцание.";
                }

            }
        }

        private async Task FLICK()
        {
            while (flick && on)
            {
                await Flashlight.TurnOnAsync();
                await Task.Delay((int)Math.Round(3000 / slider.Value, 0));
                await Flashlight.TurnOffAsync();
                await Task.Delay((int)Math.Round(3000 / slider.Value, 0));
            };
        }

        private async void _switch_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (!on)
            {
                btn.Text = "Выключить фонарик!";
                await Flashlight.TurnOnAsync();
            }
            else
            {
                btn.Text = "Включить фонарик!";
                await Flashlight.TurnOffAsync();
            }

            flick = !flick;
            on = !on;
        }
    }
    public class PopUpMorse : ContentPage
    {
        Label nameL, textL;
        Entry name, text;
        public PopUpMorse()
        {
            nameL = new Label();
            nameL.Text = "Введите пожалуйста название сообщения.(не больше 25 символов)";
            name = new Entry
            {
                MaxLength = 25,
                Placeholder = GetRandomNamePlaceholder() + '.'
            };
            textL = new Label();
            textL.Text = "Введите пожалуйста сообщение.(не больше 50 символов)";
            text = new Entry
            {
                MaxLength = 50,
                Placeholder = GetRandomPlaceholder() + "..."
            };
            Button send = new Button();
            send.Text = "Сохранить!";
            send.Clicked += Send_Clicked;
            Content = new StackLayout
            {
                Children = {nameL, name, text, textL, send}
            };
        }

        private async void Send_Clicked(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(name.Text) && !String.IsNullOrEmpty(text.Text))
            {
                MorseCode.InitializeDictionary();
                MorseCodeTemplate msg = new MorseCodeTemplate();
                msg.Text = text.Text;
                msg.Name = name.Text;
                msg.Morse = JsonConvert.SerializeObject(MorseCode.Translate(text.Text));
                App.Database.SaveItem(msg);
                await Navigation.PopAsync();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Ошибка", "Невозможно сохранить пустые название/текст", "OK");
            }
        }

        private string GetRandomPlaceholder()
        {
            string[] placeholders = new string[]
            {
                "5 минут полет нормально",
                "Первый первый, это второй",
                "Cmon boyyy",
                "Фишман встань мид",
                "Нет, друг, я не оправдываюсь",
                "БАУНТИ ХААААНТЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕЕР",
                "Борщь с капусткой, но не красный",
                "Я ща сяду за руль и ты вылетишь отсюда",
                "Ломай меня полность, я хочу чтобы ты ломал меня"
            };
            Random rnd = new Random();
            return placeholders[rnd.Next(placeholders.Length - 1)];
        }
        private string GetRandomNamePlaceholder()
        {
            string[] names = new string[]
            {
                "Полет космонавта",
                "ВВС китая",
                "Подземелье",
                "Сообщение дотеру",
                "Сообщение дотеру 2",
                "Сообщение дотеру 3",
                "Столовая",
                "Гонки",
                "Странные пожелания"
            };
            Random rnd = new Random();
            return names[rnd.Next(names.Length - 1)];
        }
    }
    public class WatchPage: ContentPage
    {
        Label textL, nameL;
        Entry text, name;
        public WatchPage(MorseCodeTemplate code)
        {
            nameL = new Label();
            nameL.Text = "Название";
            name = new Entry
            {
                MaxLength = 25,
                Text = code.Name
            };
            textL = new Label();
            textL.Text = "Сообщение";
            text = new Entry
            {
                MaxLength = 50,
                Text = code.Text
            };
            Button Ok = new Button();
            Ok.Text = "OK";
            Ok.Clicked += async (e, v) => { await Navigation.PopAsync(); };
            Content = new StackLayout
            {
                Children = { nameL, name, textL, text, Ok}
            };
        }
    }
}
