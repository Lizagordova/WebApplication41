using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication41.Models
{
    public class CurrentActions
    {

        public int CurrentActionsId { get; set; }
        public long TelegramId { get; set; }
        public int EnterWithEventCode { get; set; }
        public int PrivateCabinet { get; set; }
        public int EventCode { get; set; }
        public int CheckEmail { get; set; }
        public int NameAndLastName { get; set; }
        public int Email { get; set; }
        public int AboutEvent { get; set; }
        public int Networking { get; set; } 
        public int NetworkingFull { get; set; } 
        public int Work { get; set; } 
        public int Position { get; set; }
        public int Usefulness { get; set; } = 0;
        public int AboutWishes { get; set; } = 0;
        public int ChoseTag { get; set; } =0;
        public int ChoseSubtags { get; set; } = 0;
        public int ChoseTagAboutOthers { get; set; } = 0;
        public int ChoseSubtagsAboutOthers { get; set; } =0;
        public int ChosenTagAboutOthers { get; set; }
        public int WatchChosen { get; set; } = 0;
        public int amountForTinder { get; set; } = 0;
        public int lengthOfUsersBySubtags { get; set; }
        public int NoteBook { get; set; } = 0;
        public int MyProfile { get; set; } = 0;
        public int editName { get; set; } = 0;
        public int editWork { get; set; } = 0;
        public int editPosition { get; set; } = 0;
        public int editAboutWishes { get; set; } = 0;
        public int editUsefulness { get; set; } = 0;
        public int editTags { get; set; } =0;
        public int EditTag { get; set; } = 0;
        public int EditSubtag { get; set; } = 0;
        public int EditTagAboutOthers { get; set; } = 0;
        public int EditSubtagAboutOthers { get; set; } = 0;
        public int ChoseOldEvent { get; set; } = 0;
        public int AddTag { get; set; } = 0;
        public int AddSubtag { get; set; } = 0;
        public int AddTagAboutOthers { get; set; } = 0;
        public int AddSubtagAboutOthers { get; set; } = 0;
        public int EnteranceForOrganisators { get; set; } = 0;
        public int AddInformationAboutEvent { get; set; } = 0;
        public int CreateNotification { get; set; } = 0;
        public int CreateSurvey { get; set; } = 0;
        public int AddQuestionToSurvey { get; set; } = 0;
        public int NumberOfSurvey{ get; set; } = 0;
        public int CurrentTag { get; set; }

    }
}
