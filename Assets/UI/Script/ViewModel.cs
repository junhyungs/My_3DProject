using System.ComponentModel;

public class ViewModel
{
    private static ViewModel instance;

    public static ViewModel Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new ViewModel();
            }

            return instance;
        }
    }

    private int _currentHealth;
    private int _soulCount;
    private int _healthItemCount;
    private int _currentSkillCount;

    private PlayerSkill _currentSkill;

    public int CurrentHP
    {
        get { return _currentHealth; }
        set
        {
            if(_currentHealth == value)
            {
                return;
            }

            _currentHealth = value;

            OnPropertyChanged(nameof(CurrentHP));
        }
    }

    public int SoulCount
    {
        get { return _soulCount; }
        set
        {
            if(_soulCount == value)
            {
                return;
            }

            _soulCount = value;

            OnPropertyChanged(nameof(SoulCount));
        }
    }

    public int HealthItemCount
    {
        get { return _healthItemCount; }
        set
        {
            if( _healthItemCount == value)
            {
                return;
            }

            _healthItemCount = value;

            OnPropertyChanged(nameof(HealthItemCount));
        }
    }

    public int CurrentSkillCount
    {
        get { return _currentSkillCount; }
        set
        {
            if(_currentSkillCount == value)
            {
                return;
            }

            _currentSkillCount = value;

            OnPropertyChanged(nameof(CurrentSkillCount));
        }
    }

    public PlayerSkill CurrentSkill
    {
        get { return _currentSkill; }
        set
        {
            if(_currentSkill == value)
            {
                return;
            }

            _currentSkill = value;

            OnPropertyChanged(nameof(CurrentSkill));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
