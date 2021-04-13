using UnityEngine;

namespace AirCoder.Examples.Example2.Scripts
{
    public class UIExample : MonoBehaviour
    {
        [SerializeField] private UIPanel panel1;
        [SerializeField] private UIPanel panel2;
        [SerializeField] private UIPanel panel3;

        private UIPanel _currentPanel;
        void Start()
        {
            panel1.Initialize();
            panel1.ClosePanelImmediately();
            
            panel2.Initialize();
            
            panel3.Initialize();
            panel3.ClosePanelImmediately();
        }

        public void OpenPanel1() => OpenPanel(panel1);
        public void OpenPanel2() => OpenPanel(panel2);
        public void OpenPanel3() => OpenPanel(panel3);

        private void OpenPanel(UIPanel inPanel)
        {
            if (_currentPanel != null)
            {
                if (_currentPanel == inPanel)
                {
                    _currentPanel.ClosePanel(() => { _currentPanel = null; });
                }
                else
                {
                    _currentPanel.ClosePanel(() =>
                    {
                        inPanel.OpenPanel();
                        _currentPanel = inPanel;
                    });
                }
               
                return;
            }
            inPanel.OpenPanel();
            _currentPanel = inPanel;
        }
    }
}
