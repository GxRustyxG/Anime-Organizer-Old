﻿using Anime_Organizer.Classes;
using Anime_Organizer.Windows.Other;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace Anime_Organizer.Windows.Adding
{
    /// <summary>
    /// Interaction logic for AddNonMALAnime.xaml
    /// </summary>
    public partial class AddNonMALAnime : UserControl
    {
        public AddNonMALAnime()
        {
            InitializeComponent();

            broadcast.timeLabel.Content = TimeInfo.AbbreviationOfLocalTimeZone();

            category.categoryCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            category.categoryCBox.SelectionChanged += Category_SelectionChanged;
            titles.nicknameTBox.TextChanged += ParameterChanged.General_TextChanged;
            titles.mainTBox.TextChanged += ParameterChanged.General_TextChanged;
            titles.altTBox.TextChanged += ParameterChanged.General_TextChanged;
            websites.websiteCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            websites.CreateItems();
            score.scoreCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            score.scoreLabel.IsEnabled = false;
            score.scoreCBox.IsEnabled = false;
            score.CreateItems();
            type.typeCBox.SelectionChanged += Type_SelectionChanged;
            type.typeCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            currentlyAiring.airingCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            numOfEpisodes.episodesTBox.TextChanged += ParameterChanged.General_TextChanged;
            broadcast.daysCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            broadcast.hoursCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            broadcast.minutesCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            broadcast.dayNightCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            broadcast.checkMilitaryTime();
            premiere.premiereDayCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            premiere.premiereMonthCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            premiere.premiereYearCBox.SelectionChanged += ParameterChanged.General_SelectionChanged;
            description.descriptionTBox.TextChanged += ParameterChanged.General_TextChanged;
            confirmCancel.confirm.Click += Create_Anime_Click;
            confirmCancel.cancel.Click += Cancel_Click;
            confirmCancel.confirm.Content = "Create";

            ((MainWindow)Application.Current.MainWindow).websiteGrid.IsVisibleChanged += websiteGrid_IsVisibleChanged;

            scrollViewer.PreviewMouseWheel += new ScrollViewerTimer().PreviewMouseWheel;

            if (Config.useFont.Source != new FontFamily(new Uri("pack://application:,,,/Anime Organizer;component/Fonts/"), "./#CC Wild Words").Source)
                this.FontFamily = Config.useFont;
        }

        /// <summary>
        /// Disables broadcast control if specific type is chosen. (tv)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Type_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (!String.Equals(((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString(), "tv", StringComparison.CurrentCultureIgnoreCase))
                broadcast.IsEnabled = false;
            else
                broadcast.IsEnabled = true;
        }

        /// <summary>
        /// Disables score control based on certain conditions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Category_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (category.categoryCBox.SelectedIndex == 4)
            {
                score.scoreLabel.IsEnabled = true;
                score.scoreCBox.IsEnabled = true;
            }
            else
            {
                score.scoreCBox.IsEnabled = false;
                score.scoreCBox.IsEnabled = false;
                score.scoreCBox.SelectedIndex = -1;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /**
         * OVA: Broadcast (Makes sense since it was just posted whenever really)
         * - Links I used for Reference:
         * https://myanimelist.net/anime/7669/Bungaku_Shoujo_Kyou_no_Oyatsu__Hatsukoi
         * https://myanimelist.net/anime/32484/A-Size_Classmate
         * 
         * 
         * So ONA are generally normal sized anime but was never aired, but intended for noly the internet.
         * ONA: Broadcast
         * - Links I used for Reference:
         * https://myanimelist.net/anime/36904/Aggressive_Retsuko_ONA
         * https://myanimelist.net/anime/41555/Aikatsu_on_Parade_ONA
         * https://myanimelist.net/anime/40835/Aishen_Qiaokeli-ing__Wanjie_Jinian
         * 
         * Movie: Reference was weathering with you as it was the same as older ones.
         * 
         * It seems all of them don't have broadcast except an anime that was aired on TV.
         */


        /// <summary>
        /// Attempts to create an anime.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Create_Anime_Click(object sender, RoutedEventArgs e)
        {
            bool satisfied = true;
            Anime anime = new Anime();

            if (category.categoryCBox.Text.Length == 0)
            {
                category.categoryCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (titles.nicknameTBox.Text.Length == 0)
            {
                titles.nicknameTBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (score.scoreCBox.IsEnabled && score.scoreCBox.Text.Length == 0)
            {
                score.scoreCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }


            // Start of creation of season

            if (titles.mainTBox.Text.Length == 0 || titles.mainTBox.Text.Equals("(Japanese Pronunciation)"))
            {
                titles.mainTBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (titles.altTBox.Text.Length == 0 || titles.altTBox.Text.Equals("(English Translation)"))
            {
                titles.altTBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (websites.websiteCBox.Text.Length == 0)
            {
                websites.websiteCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (type.typeCBox.Text.Length == 0)
            {
                type.typeCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (currentlyAiring.airingCBox.Text.Length == 0)
            {
                currentlyAiring.airingCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (numOfEpisodes.episodesTBox.Text.Length == 0)
            {
                numOfEpisodes.episodesTBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            int newDay = broadcast.daysCBox.SelectedIndex, newHour = 0, newMinute = 0;

            if (type.typeCBox.SelectedIndex == -1 || String.Equals(type.typeCBox.Text, "tv", StringComparison.CurrentCultureIgnoreCase))
            {
                if (broadcast.daysCBox.Text.Length == 0)
                {
                    broadcast.daysCBox.BorderBrush = Brushes.Red;
                    satisfied = false;
                }

                if (broadcast.hoursCBox.Text.Length == 0)
                {
                    broadcast.hoursCBox.BorderBrush = Brushes.Red;
                    satisfied = false;
                }

                if (broadcast.minutesCBox.Text.Length == 0)
                {
                    broadcast.minutesCBox.BorderBrush = Brushes.Red;
                    satisfied = false;
                }

                if (!Config.isMilitaryTime && broadcast.dayNightCBox.Text.Length == 0)
                {
                    broadcast.dayNightCBox.BorderBrush = Brushes.Red;
                    satisfied = false;
                }
            }


            ////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (premiere.premiereMonthCBox.Text.Length == 0)
            {
                premiere.premiereMonthCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (premiere.premiereDayCBox.Text.Length == 0)
            {
                premiere.premiereDayCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (premiere.premiereYearCBox.Text.Length == 0)
            {
                premiere.premiereYearCBox.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            if (tags.tagPanel.Children.Count == 0)
            {
                tags.borderForWrapper.BorderBrush = Brushes.Red;
                satisfied = false;
            }

            // If it gets in here, that means at this point everything should be good to go and nothing should break.
            if (satisfied)
            {
                anime.NickNameTitle = titles.nicknameTBox.Text;
                anime.Notes = "";

                if (!score.scoreCBox.IsEnabled)
                    anime.Score = -1;
                else
                    anime.Score = double.Parse(score.scoreCBox.Text);


                // Creation of Season variable.
                Season season = new Season();

                season.MainTitle = titles.mainTBox.Text;
                season.AlternateTitle = titles.altTBox.Text;
                season.Website = websites.websiteCBox.Text;
                season.Type = type.typeCBox.Text;
                season.Airing = bool.Parse(currentlyAiring.airingCBox.Text);

                if (double.Parse(numOfEpisodes.episodesTBox.Text) == 0)
                    season.Episodes = -1;
                else
                    season.Episodes = double.Parse(numOfEpisodes.episodesTBox.Text);


                if (String.Equals(type.typeCBox.Text, "tv", StringComparison.CurrentCultureIgnoreCase))
                {
                    newHour = int.Parse(broadcast.hoursCBox.Text);
                    newMinute = int.Parse(broadcast.minutesCBox.Text);

                    // The values given should be in the users current timezone.
                    int localoffset = DateTimeOffset.Now.Offset.Hours * -1;

                    TimeInfo newTimeInfo = new TimeInfo();

                    newTimeInfo.Day = season.Broadcast.ConvertDay(localoffset, newHour, newDay);

                    if (!Config.isMilitaryTime)
                    {
                        if (broadcast.dayNightCBox.Text.Equals("am"))
                        {
                            // This is here so if its 12am, in military that means its 00:00
                            if (newHour == 12)
                                newHour = 0;
                        }
                        else if (broadcast.dayNightCBox.Text.Equals("pm"))
                        {
                            // As long as it doesn't equal 12 (because that breaks everything), then add 12 so it can be put in proper military time.
                            if (newHour != 12)
                                newHour += 12;
                        }
                    }

                    newTimeInfo.Hour = season.Broadcast.ConvertHour(localoffset, newHour);
                    newTimeInfo.Minute = newMinute;

                    season.Broadcast = new TimeInfo(newTimeInfo.Day, newTimeInfo.Hour, newTimeInfo.Minute);
                    season.Broadcast.Year = -1;
                    season.Broadcast.Month = -1;
                }
                else
                {
                    // Creation of Broadcast.
                    season.Broadcast = new TimeInfo();
                    season.Broadcast.Year = -1;
                    season.Broadcast.Month = -1;
                    season.Broadcast.Day = 0;
                    season.Broadcast.Hour = 8;
                    season.Broadcast.Minute = 0;
                }


                if (score.scoreCBox.IsEnabled == false) //change this to a !IsEnabled.
                    season.LastWatched = new LastEPWatched(0, 0);
                else if (score.scoreCBox.IsEnabled)
                    season.LastWatched = new LastEPWatched(double.Parse(numOfEpisodes.episodesTBox.Text), 0); // This might change depending on if its finished or not.


                int nonMalIDNumber = int.Parse(File.ReadAllText(Config.environmentPath + "Profiles\\Profile " + Config.profileNumber + "\\nonMALid.txt")) + 1;

                if (File.Exists(imageControl.imageTBox.Text))
                {
                    // Now we need to download the file and name it properly then save it.
                    System.Drawing.Image image = System.Drawing.Image.FromFile(imageControl.imageTBox.Text);
                    image.Save(Config.environmentPath + "Profiles\\Profile " + Config.profileNumber + "\\nonMAL Image Cache\\A" + nonMalIDNumber + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                    season.ImageURL = Config.environmentPath + "Profiles\\Profile " + Config.profileNumber + "\\nonMAL Image Cache\\A" + nonMalIDNumber + ".jpg";
                }
                else
                    season.ImageURL = "";

                season.MalId = "A" + nonMalIDNumber;

                // This saves the exact number into that file so the next MAL ID will be correct
                File.WriteAllText(Config.environmentPath + "Profiles\\Profile " + Config.profileNumber + "\\nonMALid.txt", nonMalIDNumber.ToString());

                ///////////////////////////////////////////////////////////

                season.Tags = new List<String>();

                foreach (UIElement element in tags.tagPanel.Children)
                    season.Tags.Add(((Button)element).Content.ToString());

                season.Tags.Sort();

                ///////////////////////////////////////////////////////////

                if (description.descriptionTBox.Text.Length == 0)
                    season.Description = "";
                else
                    season.Description = description.descriptionTBox.Text;

                season.Premiered = premiere.premiereMonthCBox.Text + " " + premiere.premiereDayCBox.Text + ", " + premiere.premiereYearCBox.Text;

                if (category.categoryCBox.SelectedIndex == 0)
                {
                    DateTime date = DateTime.Now;
                    anime.StartDate = new TimeInfo();
                    anime.StartDate.Year = date.Year;
                    anime.StartDate.Month = date.Month;
                    anime.StartDate.Day = date.Day;
                    anime.StartDate.Hour = -1;
                    anime.StartDate.Minute = -1;
                }

                if (category.categoryCBox.SelectedIndex == 4)
                {
                    DateTime date = DateTime.Now;

                    anime.StartDate = new TimeInfo();
                    anime.StartDate.Year = date.Year;
                    anime.StartDate.Month = date.Month;
                    anime.StartDate.Day = date.Day;
                    anime.StartDate.Hour = -1;
                    anime.StartDate.Minute = -1;

                    anime.FinishDate = new TimeInfo();
                    anime.FinishDate.Year = date.Year;
                    anime.FinishDate.Month = date.Month;
                    anime.FinishDate.Day = date.Day;
                    anime.FinishDate.Hour = -1;
                    anime.FinishDate.Minute = -1;
                }

                // At this point the anime variable is now created and ready to go.

                anime.addSeason(season);
                anime.CurrentSeason = season;
                anime.CurrentSeasonIndex = 0;

                Anime.animeLists[category.categoryCBox.SelectedIndex].Add(anime);
                Anime.animeLists[6].Add(anime);
                Config.changesSaved = false;

                ((MainWindow)Application.Current.MainWindow).websiteGrid.IsVisibleChanged -= websiteGrid_IsVisibleChanged;

                ((Grid)this.Parent).Visibility = Visibility.Collapsed;
                ((Grid)this.Parent).Children.Remove(this);
            }


        }

        /// <summary>
        /// Closes this control and goes back to the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).websiteGrid.IsVisibleChanged -= websiteGrid_IsVisibleChanged;

            ((Grid)this.Parent).Visibility = Visibility.Collapsed;
            ((Grid)this.Parent).Children.Remove(this);
        }

        /// <summary>
        /// Changes tab navigation to avoid issues.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void websiteGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // If true, removes all navigatable controls to avoid weirdness
            if ((bool)e.NewValue)
            {
                category.categoryCBox.IsTabStop = false;
                score.scoreCBox.IsTabStop = false;
                titles.nicknameTBox.IsTabStop = false;
                titles.mainTBox.IsTabStop = false;
                titles.altTBox.IsTabStop = false;
                titles.setNick1.IsTabStop = false;
                titles.setNick2.IsTabStop = false;
                websites.websiteCBox.IsTabStop = false;
                websites.websitesButton.IsTabStop = false;
                type.typeCBox.IsTabStop = false;
                currentlyAiring.airingCBox.IsTabStop = false;
                numOfEpisodes.episodesTBox.IsTabStop = false;
                broadcast.daysCBox.IsTabStop = false;
                broadcast.hoursCBox.IsTabStop = false;
                broadcast.minutesCBox.IsTabStop = false;
                broadcast.dayNightCBox.IsTabStop = false;
                premiere.premiereMonthCBox.IsTabStop = false;
                premiere.premiereDayCBox.IsTabStop = false;
                premiere.premiereYearCBox.IsTabStop = false;
                imageControl.imageTBox.IsTabStop = false;
                imageControl.browse.IsTabStop = false;
                tags.tagTBox.IsTabStop = false;
                tags.addTagButton.IsTabStop = false;
                description.descriptionTBox.IsTabStop = false;
                confirmCancel.confirm.IsTabStop = false;
                confirmCancel.cancel.IsTabStop = false;
            }
            else
            {
                category.categoryCBox.IsTabStop = true;
                score.scoreCBox.IsTabStop = true;
                titles.nicknameTBox.IsTabStop = true;
                titles.mainTBox.IsTabStop = true;
                titles.altTBox.IsTabStop = true;
                titles.setNick1.IsTabStop = true;
                titles.setNick2.IsTabStop = true;
                websites.websiteCBox.IsTabStop = true;
                websites.websitesButton.IsTabStop = true;
                type.typeCBox.IsTabStop = true;
                currentlyAiring.airingCBox.IsTabStop = true;
                numOfEpisodes.episodesTBox.IsTabStop = true;
                broadcast.daysCBox.IsTabStop = true;
                broadcast.hoursCBox.IsTabStop = true;
                broadcast.minutesCBox.IsTabStop = true;
                broadcast.dayNightCBox.IsTabStop = true;
                premiere.premiereMonthCBox.IsTabStop = true;
                premiere.premiereDayCBox.IsTabStop = true;
                premiere.premiereYearCBox.IsTabStop = true;
                imageControl.imageTBox.IsTabStop = true;
                imageControl.browse.IsTabStop = true;
                tags.tagTBox.IsTabStop = true;
                tags.addTagButton.IsTabStop = true;
                description.descriptionTBox.IsTabStop = true;
                confirmCancel.confirm.IsTabStop = true;
                confirmCancel.cancel.IsTabStop = true;
            }
        }



    }
}
