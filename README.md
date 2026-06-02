# XRInterface

Unity XR 환경에서 텔레포트 입력을 즉시 위치 이동으로 처리하지 않고, `NavMeshAgent` 기반의 자연스러운 경로 이동으로 변환한 VR 인터랙션 프로토타입입니다. XR Interaction Toolkit의 텔레포트 흐름을 확장해 목적지를 선택하면 NavMesh 위의 유효한 위치를 샘플링하고, 에이전트가 해당 지점까지 이동하도록 구성했습니다.

## 프로젝트 개요

이 프로젝트의 핵심 목표는 VR 이동 방식에서 흔히 발생하는 "순간이동 중심의 단절감"을 줄이고, 사용자가 선택한 목적지까지 캐릭터 또는 이동 주체가 경로를 따라 움직이도록 만드는 것입니다. 기본 XR 텔레포트 시스템을 그대로 사용하는 대신, 텔레포트 요청을 받아 NavMesh 기반 이동 명령으로 바꾸는 커스텀 Provider를 구현했습니다.

또한 손 제스처나 UI 이벤트와 연동할 수 있도록 이동 속도 감속, 정지, 속도 복구 로직을 분리하고, HUD에서 현재 이동 속도와 배터리 상태를 표시하는 간단한 피드백 시스템을 추가했습니다.

## 주요 기능

- XR Interaction Toolkit의 `TeleportationProvider`를 상속한 커스텀 이동 Provider
- 텔레포트 목적지를 `NavMesh.SamplePosition`으로 검증한 뒤 `NavMeshAgent.SetDestination`에 전달
- 텔레포트 요청 후 이동 시작까지 지연 시간을 둘 수 있는 이동 준비 흐름
- 이동 중 감속, 정지, 속도 복구를 담당하는 제스처 제어 컴포넌트
- TextMeshPro 기반 HUD를 통해 현재 속도와 배터리 잔량 표시
- Unity Navigation, OpenXR, XR Hands, XR Interaction Toolkit을 함께 사용하는 XR 실험 환경

## 기술 스택

| 구분 | 내용 |
| --- | --- |
| Engine | Unity 6000.0.41f1 |
| Rendering | Universal Render Pipeline 17.0.4 |
| XR | XR Interaction Toolkit 3.1.1, XR Management 4.5.0, OpenXR 1.14.1 |
| Input | Input System 1.13.1, XR Hands 1.5.0 |
| Navigation | AI Navigation 2.0.8, NavMeshAgent |
| UI | TextMeshPro |
| Language | C# |

## 핵심 구현

### NavMesh 기반 텔레포트 Provider

`NavMeshProvider`는 XR Interaction Toolkit의 `TeleportationProvider`를 확장합니다. 기존 텔레포트처럼 XR Origin을 즉시 옮기는 대신, 텔레포트 요청의 목적지를 받아 NavMesh에서 유효한 위치인지 확인하고 `NavMeshAgent`의 목적지로 설정합니다.

```csharp
if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
{
    navMeshAgent.SetDestination(hit.position);
    navGestureController.RestoreSpeed();
}
```

이 구조 덕분에 XR 입력 체계는 유지하면서도 실제 이동 방식은 NavMesh 경로 탐색으로 바꿀 수 있습니다.

### 이동 제어와 제스처 연동

`NavGestureController`는 이동 중 발생할 수 있는 제어 이벤트를 담당합니다. 현재 경로를 초기화하고 감속하거나, 에이전트를 정지시키거나, 새로운 이동 요청이 들어왔을 때 원래 속도를 복구합니다.

이 컴포넌트는 씬의 손 제스처 이벤트 또는 XR 상호작용 이벤트와 연결해 이동 상태를 제어하는 용도로 사용할 수 있습니다.

### HUD 피드백

`NavAgentHUDUpdater`는 `NavMeshAgent`의 현재 속도를 읽어 HUD에 표시하고, 시간 경과에 따라 감소하는 배터리 값을 TextMeshPro UI에 갱신합니다. 이동 상태를 사용자가 바로 인지할 수 있도록 만든 보조 피드백 시스템입니다.

## 프로젝트 구조

```text
XRInterface/
  Assets/
    Scenes/
      SampleScene.unity
    Scripts/
      NavMeshProvider.cs
      NavGestureController.cs
      NavAgentHUDUpdater.cs
    XR/
    XRI/
    Samples/
  Packages/
    manifest.json
  ProjectSettings/
    ProjectVersion.txt
```

## 스크립트 역할

| 파일 | 역할 |
| --- | --- |
| `NavMeshProvider.cs` | XR 텔레포트 요청을 NavMeshAgent 이동 명령으로 변환 |
| `NavGestureController.cs` | 이동 감속, 정지, 속도 복구 제어 |
| `NavAgentHUDUpdater.cs` | 에이전트 속도와 배터리 정보를 HUD에 표시 |

## 실행 방법

1. Unity Hub에서 `XRInterface/` 폴더를 프로젝트로 엽니다.
2. Unity 버전은 `6000.0.41f1`을 권장합니다.
3. 패키지 복원이 완료될 때까지 기다립니다.
4. `Assets/Scenes/SampleScene.unity`를 엽니다.
5. XR 장비 또는 XR Device Simulator 환경에서 씬을 실행합니다.
