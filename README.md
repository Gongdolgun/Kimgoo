![header](https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=header&text=Kimgoo&fontSize=60)

# Kimgoo

### 프로젝트 기간
2022.01 ~ 2022.06

### 기술 스택
<img src="https://img.shields.io/badge/Unity-000000?style=flat-square&logo=Unity&logoColor=white"/>  <img src="https://img.shields.io/badge/C Sharp-239120?style=flat-square&logo=C Sharp&logoColor=white"/>  <img src="https://img.shields.io/badge/Google Dialogflow-FF9800?style=flat-square&logo=Dialogflow&logoColor=white"/>  <img src="https://img.shields.io/badge/Synology-B5B5B6?style=flat-square&logo=Synology&logoColor=white"/>

### 아키텍처
<img width="80%" src="https://user-images.githubusercontent.com/90584581/197328141-caef4475-c90d-4eee-88da-de2217ea4a36.png"/>

### 프로젝트 내용
인간형 AI와 실시간으로 대화하며 백범 김구 선생님의 과거와 역사적 사실에 대해 알아갈 수 있는 프로젝트입니다.\
리얼센스 카메라를 이용하여 키오스크 앞에 사람이 있는지 확인한 후 사람이 인식되면 시작되고 사라지면 다시 처음으로 돌아갑니다.\
현재 용산, 화성, 광주에서 전시중이며 지속적인 클라이언트와의 커뮤니케이션으로 유지보수중입니다.

<img width="20%" src="https://user-images.githubusercontent.com/90584581/196046199-d2346e11-0d1c-4296-a13f-05bd8a555c56.jpg"/>  <img width="20%" src="https://user-images.githubusercontent.com/90584581/196109249-b1e69425-8b48-4dbd-aea9-67989ca5cabc.jpg"/>  <img width="20%" src="https://user-images.githubusercontent.com/90584581/196110242-de887c1f-9d46-4468-b6c0-1a555d09ec11.png"/>  <img width="20%" src="https://user-images.githubusercontent.com/90584581/196113519-f7067fae-81dd-4f86-ab42-0a9eb529e259.jpg"/>

### 프로젝트 투입 인원
개발자 2, 디자이너 1

### 나의 역할
- Google Dialogflow API 연동
- 코드 결합 및 수정
- 프로젝트 QA
- 프로젝트 유지보수
- 클라이언트와 지속적으로 면담 및 요구사항 수정

### 핵심 코드
Google Dialogflow에서 온 응답을 적용하는 코드입니다.\
response의 값을 가지고 애니메이션, 음성파일등 전체적인 프로젝트의 흐름을 핸들링합니다.

        ```
        private void LogResponseText(DF2Response response)
	    {
		    chatbotText.text = response.queryResult.queryText +"\n";
		    chatbotText.text += response.queryResult.fulfillmentText;
		    StartCoroutine(SetFinish(response.queryResult.queryText, response.queryResult.fulfillmentText, "1001"));
		    if (response.queryResult.fulfillmentText != null)
		    {
			    Debug.Log(response.queryResult.fulfillmentText);
			    if (response.queryResult.fulfillmentText == "10002")
			    {
				    Debug.Log(GetSessionName() + " said: \"" + response.queryResult.fulfillmentText + "\"");
				    uIOnOff.SetAxtive();
				    micInput.InitMic();
				    Debug.Log(" null");
			    }

			    else
			    {
				    micInput.ResponseControl(int.Parse(response.queryResult.fulfillmentText));
			    }
		    }
		    else
		    {
			    micInput.ResponseControl();
			    Debug.Log(" null");
		    }
	    }
        ```


![Footer](https://capsule-render.vercel.app/api?type=waving&color=auto&height=200&section=footer)
