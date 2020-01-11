using BertScout2020.ViewModels;
using BertScout2020Data.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BertScout2020.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditEventTeamMatchPage : ContentPage
    {
        EditEventTeamMatchViewModel viewModel;

        public EditEventTeamMatchPage(EventTeamMatch item)
        {
            InitializeComponent();

            if (item.Changed % 2 == 0) // change from even to odd, odd = must upload
            {
                item.Changed++;
            }

            BindingContext = viewModel = new EditEventTeamMatchViewModel(item);

#if DEBUG
            LabelChangeVersion.Text = $"Change Version: {item.Changed}";
            LabelChangeVersion.IsVisible = true;
#endif

            SetButtons();
        }

        private void SetButtons()
        {
            AutoStartPos = viewModel.item.AutoStartPos;
            AutoLeaveInitLine = viewModel.item.AutoLeaveInitLine;
            AutoLowCell = viewModel.item.AutoLowCell;
            AutoHighCell = viewModel.item.AutoHighCell;

            AutoInnerCell = viewModel.item.AutoInnerCell;
            TeleLowCell = viewModel.item.TeleLowCell;
            TeleHighCell = viewModel.item.TeleHighCell;
            TeleInnerCell = viewModel.item.TeleInnerCell;
            RotationControl = viewModel.item.RotationControl;
            PositionControl = viewModel.item.PositionControl;

            ClimbStatus = viewModel.item.ClimbStatus;
            LevelSwitch = viewModel.item.LevelSwitch;

            Fouls = viewModel.item.Fouls;
            Broken = viewModel.item.Broken;

            AllianceResult = viewModel.item.AllianceResult;
            StageRankingPoint = viewModel.item.StageRankingPoint;
            ClimbRankingPoint = viewModel.item.ClimbRankingPoint;
        }

        #region AutoStartPos

        public int AutoStartPos
        {
            get
            {
                return viewModel.item.AutoStartPos;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_AutoStartPos_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoStartPos_Auto.BackgroundColor = App.SelectedButtonColor;
                        Button_AutoStartPos_Tele.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_AutoStartPos_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoStartPos_Auto.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoStartPos_Tele.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_AutoStartPos_None.BackgroundColor = App.SelectedButtonColor;
                        Button_AutoStartPos_Auto.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoStartPos_Tele.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.AutoStartPos != newValue)
                {
                    viewModel.item.AutoStartPos = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AutoStartPos_None_Clicked(object sender, System.EventArgs e)
        {
            AutoStartPos = 0;
        }

        private void Button_AutoStartPos_Auto_Clicked(object sender, System.EventArgs e)
        {
            AutoStartPos = 1;
        }

        private void Button_AutoStartPos_Tele_Clicked(object sender, System.EventArgs e)
        {
            AutoStartPos = 2;
        }

        #endregion

        #region AutoLeaveInitLine

        public int AutoLeaveInitLine
        {
            get
            {
                return viewModel.item.AutoLeaveInitLine;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_AutoLeaveInitLine_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoLeaveInitLine_1.BackgroundColor = App.SelectedButtonColor;
                        Button_AutoLeaveInitLine_2.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_AutoLeaveInitLine_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoLeaveInitLine_1.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoLeaveInitLine_2.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_AutoLeaveInitLine_None.BackgroundColor = App.SelectedButtonColor;
                        Button_AutoLeaveInitLine_1.BackgroundColor = App.UnselectedButtonColor;
                        Button_AutoLeaveInitLine_2.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.AutoLeaveInitLine != newValue)
                {
                    viewModel.item.AutoLeaveInitLine = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AutoLeaveInitLine_None_Clicked(object sender, System.EventArgs e)
        {
            AutoLeaveInitLine = 0;
        }

        private void Button_AutoLeaveInitLine_1_Clicked(object sender, System.EventArgs e)
        {
            AutoLeaveInitLine = 1;
        }

        private void Button_AutoLeaveInitLine_2_Clicked(object sender, System.EventArgs e)
        {
            AutoLeaveInitLine = 2;
        }

        #endregion

        #region AutoLowCell

        public int AutoLowCell
        {
            get
            {
                return viewModel.item.AutoLowCell;
            }
            set
            {
                Label_AutoLowCell_Value.Text = value.ToString();
                if (viewModel.item.AutoLowCell != value)
                {
                    viewModel.item.AutoLowCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AutoLowCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoLowCell > 0)
            {
                AutoLowCell--;
            }
        }

        private void Button_AutoLowCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoLowCell < 99)
            {
                AutoLowCell++;
            }
        }

        #endregion

        #region AutoHighCell

        public int AutoHighCell
        {
            get
            {
                return viewModel.item.AutoHighCell;
            }
            set
            {
                Label_AutoHighCell_Value.Text = value.ToString();
                if (viewModel.item.AutoHighCell != value)
                {
                    viewModel.item.AutoHighCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AutoHighCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoHighCell > 0)
            {
                AutoHighCell--;
            }
        }

        private void Button_AutoHighCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoHighCell < 99)
            {
                AutoHighCell++;
            }
        }

        #endregion

        #region AutoInnerCell

        public int AutoInnerCell
        {
            get
            {
                return viewModel.item.AutoInnerCell;
            }
            set
            {
                Label_AutoInnerCell_Value.Text = value.ToString();
                if (viewModel.item.AutoInnerCell != value)
                {
                    viewModel.item.AutoInnerCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AutoInnerCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoInnerCell > 0)
            {
                AutoInnerCell--;
            }
        }

        private void Button_AutoInnerCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (AutoInnerCell < 99)
            {
                AutoInnerCell++;
            }
        }

        #endregion

        #region TeleLowCell

        public int TeleLowCell
        {
            get
            {
                return viewModel.item.TeleLowCell;
            }
            set
            {
                Label_TeleLowCell_Value.Text = value.ToString();
                if (viewModel.item.TeleLowCell != value)
                {
                    viewModel.item.TeleLowCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_TeleLowCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleLowCell > 0)
            {
                TeleLowCell--;
            }
        }

        private void Button_TeleLowCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleLowCell < 99)
            {
                TeleLowCell++;
            }
        }

        #endregion

        #region TeleHighCell

        public int TeleHighCell
        {
            get
            {
                return viewModel.item.TeleHighCell;
            }
            set
            {
                Label_TeleHighCell_Value.Text = value.ToString();
                if (viewModel.item.TeleHighCell != value)
                {
                    viewModel.item.TeleHighCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_TeleHighCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleHighCell > 0)
            {
                TeleHighCell--;
            }
        }

        private void Button_TeleHighCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleHighCell < 99)
            {
                TeleHighCell++;
            }
        }

        #endregion

        #region TeleInnerCell

        public int TeleInnerCell
        {
            get
            {
                return viewModel.item.TeleInnerCell;
            }
            set
            {
                Label_TeleInnerCell_Value.Text = value.ToString();
                if (viewModel.item.TeleInnerCell != value)
                {
                    viewModel.item.TeleInnerCell = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_TeleInnerCell_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleInnerCell > 0)
            {
                TeleInnerCell--;
            }
        }

        private void Button_TeleInnerCell_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (TeleInnerCell < 99)
            {
                TeleInnerCell++;
            }
        }

        #endregion

        #region RotationControl

        public int RotationControl
        {
            get
            {
                return viewModel.item.RotationControl;
            }
            set
            {
                Label_RotationControl_Value.Text = value.ToString();
                if (viewModel.item.RotationControl != value)
                {
                    viewModel.item.RotationControl = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_RotationControl_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (RotationControl > 0)
            {
                RotationControl--;
            }
        }

        private void Button_RotationControl_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (RotationControl < 3)
            {
                RotationControl++;
            }
        }

        #endregion

        #region PositionControl

        public int PositionControl
        {
            get
            {
                return viewModel.item.PositionControl;
            }
            set
            {
                Label_PositionControl_Value.Text = value.ToString();
                if (viewModel.item.PositionControl != value)
                {
                    viewModel.item.PositionControl = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_PositionControl_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (PositionControl > 0)
            {
                PositionControl--;
            }
        }

        private void Button_PositionControl_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (PositionControl < 3)
            {
                PositionControl++;
            }
        }

        #endregion

        #region ClimbStatus

        public int ClimbStatus
        {
            get
            {
                return viewModel.item.ClimbStatus;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_ClimbStatus_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_1.BackgroundColor = App.SelectedButtonColor;
                        Button_ClimbStatus_2.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_3.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_ClimbStatus_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_1.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_2.BackgroundColor = App.SelectedButtonColor;
                        Button_ClimbStatus_3.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 3:
                        newValue = 3;
                        Button_ClimbStatus_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_1.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_2.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_3.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_ClimbStatus_None.BackgroundColor = App.SelectedButtonColor;
                        Button_ClimbStatus_1.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_2.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbStatus_3.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.ClimbStatus != newValue)
                {
                    viewModel.item.ClimbStatus = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_ClimbStatus_None_Clicked(object sender, System.EventArgs e)
        {
            ClimbStatus = 0;
        }

        private void Button_ClimbStatus_1_Clicked(object sender, System.EventArgs e)
        {
            ClimbStatus = 1;
        }

        private void Button_ClimbStatus_2_Clicked(object sender, System.EventArgs e)
        {
            ClimbStatus = 2;
        }

        private void Button_ClimbStatus_3_Clicked(object sender, System.EventArgs e)
        {
            ClimbStatus = 3;
        }

        #endregion

        #region LevelSwitch

        public int LevelSwitch
        {
            get
            {
                return viewModel.item.LevelSwitch;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_LevelSwitch_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_LevelSwitch_On.BackgroundColor = App.SelectedButtonColor;
                        Button_LevelSwitch_Lift.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_LevelSwitch_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_LevelSwitch_On.BackgroundColor = App.UnselectedButtonColor;
                        Button_LevelSwitch_Lift.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_LevelSwitch_None.BackgroundColor = App.SelectedButtonColor;
                        Button_LevelSwitch_On.BackgroundColor = App.UnselectedButtonColor;
                        Button_LevelSwitch_Lift.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.LevelSwitch != newValue)
                {
                    viewModel.item.LevelSwitch = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_LevelSwitch_None_Clicked(object sender, System.EventArgs e)
        {
            LevelSwitch = 0;
        }

        private void Button_LevelSwitch_On_Clicked(object sender, System.EventArgs e)
        {
            LevelSwitch = 1;
        }

        private void Button_LevelSwitch_Lift_Clicked(object sender, System.EventArgs e)
        {
            LevelSwitch = 2;
        }


        #endregion
        
        /*
        #region Defense

        public int Defense
        {
            get
            {
                return viewModel.item.Defense;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_Defense_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Defense_Some.BackgroundColor = App.SelectedButtonColor;
                        Button_Defense_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_Defense_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Defense_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Defense_Lots.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_Defense_None.BackgroundColor = App.SelectedButtonColor;
                        Button_Defense_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Defense_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.Defense != newValue)
                {
                    viewModel.item.Defense = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_Defense_None_Clicked(object sender, System.EventArgs e)
        {
            Defense = 0;
        }

        private void Button_Defense_Some_Clicked(object sender, System.EventArgs e)
        {
            Defense = 1;
        }

        private void Button_Defense_Lots_Clicked(object sender, System.EventArgs e)
        {
            Defense = 2;
        }

        #endregion

        #region Cooperation
        
        public int Cooperation
        {
            get
            {
                return viewModel.item.Cooperation;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_Cooperation_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Cooperation_Some.BackgroundColor = App.SelectedButtonColor;
                        Button_Cooperation_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_Cooperation_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Cooperation_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Cooperation_Lots.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_Cooperation_None.BackgroundColor = App.SelectedButtonColor;
                        Button_Cooperation_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Cooperation_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.Cooperation != newValue)
                {
                    viewModel.item.Cooperation = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }
        

        private void Button_Cooperation_None_Clicked(object sender, System.EventArgs e)
        {
            Cooperation = 0;
        }

        private void Button_Cooperation_Some_Clicked(object sender, System.EventArgs e)
        {
            Cooperation = 1;
        }

        private void Button_Cooperation_Lots_Clicked(object sender, System.EventArgs e)
        {
            Cooperation = 2;
        }

        #endregion
        */

        #region Fouls

        public int Fouls
        {
            get
            {
                return viewModel.item.Fouls;
            }
            set
            {
                Label_Fouls_Value.Text = value.ToString();
                if (viewModel.item.Fouls != value)
                {
                    viewModel.item.Fouls = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_Fouls_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (Fouls > 0)
            {
                Fouls--;
            }
        }

        private void Button_Fouls_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (Fouls < 99)
            {
                Fouls++;
            }
        }

        #endregion

        /*
        #region TechFouls

        public int TechFouls
        {
            get
            {
                return viewModel.item.TechFouls;
            }
            set
            {
                Label_TechFouls_Value.Text = value.ToString();
                if (viewModel.item.TechFouls != value)
                {
                    viewModel.item.TechFouls = value;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_TechFouls_Minus_Clicked(object sender, System.EventArgs e)
        {
            if (TechFouls > 0)
            {
                TechFouls--;
            }
        }

        private void Button_TechFouls_Plus_Clicked(object sender, System.EventArgs e)
        {
            if (TechFouls < 99)
            {
                TechFouls++;
            }
        }

        #endregion
        */

        #region Broken

        public int Broken
        {
            get
            {
                return viewModel.item.Broken;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_Broken_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Broken_Some.BackgroundColor = App.SelectedButtonColor;
                        Button_Broken_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_Broken_None.BackgroundColor = App.UnselectedButtonColor;
                        Button_Broken_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Broken_Lots.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_Broken_None.BackgroundColor = App.SelectedButtonColor;
                        Button_Broken_Some.BackgroundColor = App.UnselectedButtonColor;
                        Button_Broken_Lots.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.Broken != newValue)
                {
                    viewModel.item.Broken = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_Broken_None_Clicked(object sender, System.EventArgs e)
        {
            Broken = 0;
        }

        private void Button_Broken_Some_Clicked(object sender, System.EventArgs e)
        {
            Broken = 1;
        }

        private void Button_Broken_Lots_Clicked(object sender, System.EventArgs e)
        {
            Broken = 2;
        }

        #endregion

        #region AllianceResult

        public int AllianceResult
        {
            get
            {
                return viewModel.item.AllianceResult;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_AllianceResult_Lost.BackgroundColor = App.UnselectedButtonColor;
                        Button_AllianceResult_Tied.BackgroundColor = App.SelectedButtonColor;
                        Button_AllianceResult_Won.BackgroundColor = App.UnselectedButtonColor;
                        break;
                    case 2:
                        newValue = 2;
                        Button_AllianceResult_Lost.BackgroundColor = App.UnselectedButtonColor;
                        Button_AllianceResult_Tied.BackgroundColor = App.UnselectedButtonColor;
                        Button_AllianceResult_Won.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_AllianceResult_Lost.BackgroundColor = App.SelectedButtonColor;
                        Button_AllianceResult_Tied.BackgroundColor = App.UnselectedButtonColor;
                        Button_AllianceResult_Won.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.AllianceResult != newValue)
                {
                    viewModel.item.AllianceResult = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_AllianceResult_Lost_Clicked(object sender, System.EventArgs e)
        {
            AllianceResult = 0;
        }

        private void Button_AllianceResult_Tied_Clicked(object sender, System.EventArgs e)
        {
            AllianceResult = 1;
        }

        private void Button_AllianceResult_Won_Clicked(object sender, System.EventArgs e)
        {
            AllianceResult = 2;
        }

        #endregion

        #region StageRankingPoint

        public int StageRankingPoint
        {
            get
            {
                return viewModel.item.StageRankingPoint;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_StageRankingPoint_No.BackgroundColor = App.UnselectedButtonColor;
                        Button_StageRankingPoint_Yes.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_StageRankingPoint_No.BackgroundColor = App.SelectedButtonColor;
                        Button_StageRankingPoint_Yes.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.StageRankingPoint != newValue)
                {
                    viewModel.item.StageRankingPoint = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_StageRankingPoint_No_Clicked(object sender, System.EventArgs e)
        {
            StageRankingPoint = 0;
        }

        private void Button_StageRankingPoint_Yes_Clicked(object sender, System.EventArgs e)
        {
            StageRankingPoint = 1;
        }

        #endregion

        #region ClimbRankingPoint

        public int ClimbRankingPoint
        {
            get
            {
                return viewModel.item.ClimbRankingPoint;
            }
            set
            {
                int newValue = value;
                switch (value)
                {
                    case 1:
                        newValue = 1;
                        Button_ClimbRankingPoint_No.BackgroundColor = App.UnselectedButtonColor;
                        Button_ClimbRankingPoint_Yes.BackgroundColor = App.SelectedButtonColor;
                        break;
                    default:
                        newValue = 0;
                        Button_ClimbRankingPoint_No.BackgroundColor = App.SelectedButtonColor;
                        Button_ClimbRankingPoint_Yes.BackgroundColor = App.UnselectedButtonColor;
                        break;
                }
                if (viewModel.item.ClimbRankingPoint != newValue)
                {
                    viewModel.item.ClimbRankingPoint = newValue;
                    App.database.SaveEventTeamMatchAsync(viewModel.item);
                }
            }
        }

        private void Button_ClimbRankingPoint_No_Clicked(object sender, System.EventArgs e)
        {
            ClimbRankingPoint = 0;
        }

        private void Button_ClimbRankingPoint_Yes_Clicked(object sender, System.EventArgs e)
        {
            ClimbRankingPoint = 1;
        }

        #endregion

        private void ToolbarItem_Comments_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new EditMatchCommentPage(viewModel.item));
        }
    }
}