# AnimationClip_to_BVH

AnimationClip_to_BVH は、 Emiliana さんのUnityアセットである BVH Tools に追加して使うことで、
1つのAnimationClipをBVHファイルに変換することができる補助的なC#スクリプトです。(VRM形式のアバターを想定)
BVHファイルを記録・作成するなどほとんどの処理はBVH Toolsに行っていただいており、AnimationClip_to_BVHが行うのは
必要な情報入力の補助と、1アニメーションの始まりと終わりのタイミングでのカットのみです。

BVH Toolsを含まない AnimationClip_to_BVH.cs単体に関しては、商用非商用・改変・配布などを問わず自由にご使用いただいて大丈夫です。

使い方：
1. BVH ToolsをUnityアセットストアから購入(2023年10月18日時点で無料)し、Unityプロジェクトに追加します。
2. 本スクリプトAnimationClip_to_BVH.csをダウンロード・保存し、UnityプロジェクトのAssetsフォルダにドラッグ&ドロップします。
3. モーションの記録に使用したいアバター(本スクリプトではVRM形式を想定しています)をUnityのシーン上にドラッグ&ドロップします。
4. アバターのインスペクターに 本スクリプトAnimationClip_to_BVH.csをアタッチします。(自動でBVH ToolsのBVH Recorderが追加されます)
5. インスペクター上のAnimationClip_to_BVHの項目(おそらくインスペクターの最下部)に次の操作をしていきます。
<br>  5.1. "アニメーションクリップ"の項目にBVHに変換したいアニメーションクリップを設定します。
<br>  5.2. "収録フレームレート"の欄に記録に使いたいフレームレートを入力します。(下部の "アニメーションクリップのフレームレートを取得する" ボタンを押すとアニメーションオリジナルのフレームレートを使うこともできます。)
<br>  5.3. "BVHファイルを保存する場所（フォルダ）"の項目は、下部の "保存場所を選択する" ボタンから選ぶことができます
<br>  5.4. "保存するBVHファイルの名前"の欄に付けたいファイル名を入力します。拡張子ありでもなしでも可です。
<br>  5.5. 下部の"設定を適用する"ボタンを押します。
<br>  5.6. 最下部の"再生する"ボタンを押します。
12. アニメーションの記録が終わるとその旨のダイアログが出、OKを押すとPlayモードが終了します。
13. しばらく待つと任意の場所にBVHファイルが保存されると思います。


素敵なアセットを作ってくださった Emiliana さんに感謝いたします。

参考：
<br>Emiliana さん
<br>https://twitter.com/Emiliana_vt
<br>https://www.youtube.com/@Emiliana_vt
<br>https://emilianavt.github.io/
<br>https://assetstore.unity.com/publishers/40073

BVH Tools
<br>https://assetstore.unity.com/packages/tools/animation/bvh-tools-144728
<br>https://github.com/emilianavt/BVHTools
<br>https://www.youtube.com/watch?v=DM7UZuAgBJk

VRM
<br>https://vrm.dev/index.html
