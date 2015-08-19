using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using UTHPortal.Models;

namespace UTHPortal.ViewModel
{
    public class AnnounceListViewModel : UpdatableViewModel<AnnounceModel>
    {
        public AnnounceListViewModel()
        {
            if (IsInDesignMode)
            {
                Data = new AnnounceModel();
                Data.Entries = new ObservableCollection<Announce>();
                Data.Title = "Νεα Πανεπιστημίου Θεσσαλίας";
                for (int i = 0; i < 5; i++)
                {
                    Announce newEntry = new Announce();
                    newEntry.Title = "Εικονική πόλη για την από κοινού εκπαίδευση";
                    newEntry.Plaintext = "Το Πανεπιστήμιο Θεσσαλίας, Τμήμα Ηλεκτρολόγων Μηχανικών και Μηχανικών Υπολογιστών, συμμετέχει στην ανάπτυξη εικονικού περιβάλλοντος πόλης μέσα από το οποίο μαθητές και φοιτητές μπορούν να αναπτύσσουν δεξιότητες σε θετικές επιστήμες, μαθηματικά, και τεχνολογία.  Το ερευνητικό σχέδιο eCity: Virtual City Environment for Engineering Problem-based Learning (http://eCity-project.eu) χρηματοδοτείται με την υποστήριξη του προγράμματος Δια Βίου Μάθησης της Ευρωπαϊκής Επιτροπής. Συντονιστής του σχεδίου είναι το Instituto Superior de Engeharia do Porto της Πορτογαλίας ενώ συμμετέχουν, εκτός από το Πανεπιστήμιο Θεσσαλίας, οργανισμοί από την Ιταλία, Ισπανία, και Τουρκία.\nΠληροφορίες";
                    newEntry.Date = new DateTime(2015, 8-i, 7);

                    Data.Entries.Add(newEntry);
                }
            }
        }


        /// <summary>
        /// Navigates to another view in order to see the specific announcement
        /// </summary>
        public RelayCommand<Announce> ShowDetails
        {
            get
            {
                return _showDetails
                    ?? (_showDetails = new RelayCommand<Announce>(
                      entry =>
                      {
                          navigationService.NavigateTo(
                              typeof(AnnounceListDetailsView),
                              typeof(AnnounceListDetailsViewModel),
                              entry
                          );
                      }));
            }
        }
        private RelayCommand<Announce> _showDetails;

    }
}
