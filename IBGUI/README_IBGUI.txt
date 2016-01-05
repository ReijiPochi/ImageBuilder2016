使い方：
0. Livet.dllやその他dllも含めてアプリケーションのフォルダにコピー
1. 参照設定にIBGUI.dllを追加
2. App.xamlのApplication.Resourcesに
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/IBGUI;component/ResourceDictionaries.xaml" />
            </ResourceDictionary.MergedDictionaries>
   を追加