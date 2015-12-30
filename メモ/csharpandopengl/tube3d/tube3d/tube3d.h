// tube3d.h : tube3d.DLL のメイン ヘッダー ファイル
//

#pragma once

#ifndef __AFXWIN_H__
	#error "PCH に対してこのファイルをインクルードする前に 'stdafx.h' をインクルードしてください"
#endif

#include "resource.h"		// メイン シンボル


// Ctube3dApp
// このクラスの実装に関しては tube3d.cpp を参照してください。
//

class Ctube3dApp : public CWinApp
{
public:
	Ctube3dApp();

// オーバーライド
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
