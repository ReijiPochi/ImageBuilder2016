使い方：
0. OpenCL.dllやその他dllも含めてアプリケーションのフォルダにコピー
1. 参照設定にIBFramework.dllを追加
2. App.xamlのApplication.Resourcesに
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="pack://application:,,,/IBFramework;component/ResourceDictionaries.xaml" />
            </ResourceDictionary.MergedDictionaries>
   を追加
3. RedoUndoManager.Current = new RedoUndoManager();