using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTHPortal.Common;
using UTHPortal.Models;
using Windows.UI.ViewManagement;

namespace UTHPortal.ViewModel
{
    public class AnnounceListViewModel : UpdatableViewModel<AnnounceModelBase>
    {
        public AnnounceListViewModel()
        {
            if (IsInDesignMode)
            {
                Data = new AnnounceModelBase();
                Data.Entries = new ObservableCollection<Announce>();
                Data.Title = "Νεα Πανεπιστημίου Θεσσαλίας";
                for (int i = 0; i < 5; i++)
                {
                    Announce newEntry = new Announce();
                    newEntry.Title = "Εικονική πόλη για την από κοινού εκπαίδευση";
                    newEntry.Plaintext = "Το Πανεπιστήμιο Θεσσαλίας, Τμήμα Ηλεκτρολόγων Μηχανικών και Μηχανικών Υπολογιστών, συμμετέχει στην ανάπτυξη εικονικού περιβάλλοντος πόλης μέσα από το οποίο μαθητές και φοιτητές μπορούν να αναπτύσσουν δεξιότητες σε θετικές επιστήμες, μαθηματικά, και τεχνολογία.  Το ερευνητικό σχέδιο eCity: Virtual City Environment for Engineering Problem-based Learning (http://eCity-project.eu) χρηματοδοτείται με την υποστήριξη του προγράμματος Δια Βίου Μάθησης της Ευρωπαϊκής Επιτροπής. Συντονιστής του σχεδίου είναι το Instituto Superior de Engeharia do Porto της Πορτογαλίας ενώ συμμετέχουν, εκτός από το Πανεπιστήμιο Θεσσαλίας, οργανισμοί από την Ιταλία, Ισπανία, και Τουρκία.\nΠληροφορίες";
                    newEntry.Date = new DateTime(2014, 9, 23);

                    Data.Entries.Add(newEntry);
                }
            }
        }

        private RelayCommand<Announce> _showDetails;

        /// <summary>
        /// Show the UniversityRssDetailsView based on what item was tapped
        /// </summary>
        public RelayCommand<Announce> ShowDetails
        {
            get
            {
                return _showDetails
                    ?? (_showDetails = new RelayCommand<Announce>(
                                          entry =>
                                          {
                                              _navigationService.NavigateTo(
                                                  typeof(AnnounceListDetailsView),
                                                  typeof(AnnounceListDetailsViewModel),
                                                  entry);
                                          }));
            }
        }

        protected override async void ExecutePageLoaded()
        {
            if (_navigationService.StateExists(this.GetType()))
            {
                Url = (string)_navigationService.GetAndRemoveState(this.GetType());

                await GetSavedView();

                if ((bool)_storageService.GetSettingsEntry("AutoRefresh")) {
                    await DispatcherHelper.RunAsync(() => {
                        RefreshCommand.Execute(null);
                    });
                }
            }
        }

    }
}
