# VR_GAME_VUI

> Dự án VR tương tác dành cho bài tập hoặc demo học tập, được xây dựng bằng Unity với XR Interaction Toolkit và OpenXR.

## Tổng quan

`VR_GAME_VUI` là một game VR tương tác nơi người chơi dùng búa để phá cupcake và tích điểm trong nhiều giai đoạn. Dự án sử dụng Unity XR Foundation, XR Interaction Toolkit và OpenXR để hỗ trợ trải nghiệm VR thực tế trên thiết bị tương thích.

## Điểm nổi bật

- Menu VR tương tác với hiệu ứng hover, click và chuyển cảnh mượt.
- Gameplay dựa trên tương tác vật lý: người chơi cầm búa, đánh cupcake.
- Hệ thống điểm số với tiến trình, chuyển cảnh khi đạt mục tiêu.
- Nhạc nền liên tục giữa các cảnh và fade out khi kết thúc.
- Sử dụng OpenXR và Oculus plugin cho XR.

## Cấu trúc chính

- `VR_GAME/VR_GAME/Assets/` - Tài nguyên Unity chính của project.
- `VR_GAME/VR_GAME/Assets/Scripts/` - Các script gameplay chính.
- `VR_GAME/VR_GAME/Assets/EndingScene.unity` - Scene kết thúc.
- `VR_GAME/VR_GAME/Assets/MenuScene.unity` - Scene menu.
- `VR_GAME/VR_GAME/Assets/IntroScene.unity` - Scene giới thiệu.
- `VR_GAME/VR_GAME/Assets/GameScene.unity` - Scene gameplay đầu tiên.
- `VR_GAME/VR_GAME/Assets/GameSceneIntermédiaire.unity` - Scene gameplay trung gian.

## Scripts chính

- `MenuManager.cs` - Quản lý menu VR, hiệu ứng button, âm thanh và chuyển cảnh.
- `CupcakeSpawner.cs` - Sinh cupcake tại nhiều vị trí, điều khiển chuyển động lên/xuống và quản lý vòng đời cupcake.
- `HammerHit.cs` - Xử lý va chạm búa với cupcake, điểm số và hiệu ứng phá hủy.
- `ScoreManagerFirstScene.cs` - Quản lý điểm số và chuyển cảnh từ giai đoạn 0-50 điểm.
- `ScoreManager.cs` - Quản lý điểm số giai đoạn 50-100 điểm và kết thúc trò chơi.
- `MusicManager.cs` - Phát nhạc nền, singleton giữ nhạc liên tục giữa các scene và fade out.
- `MascotIntro.cs` - Quản lý hoạt cảnh mascot (nếu sử dụng trong scene giới thiệu).

## Công nghệ và dependencies

- Unity Editor: `6000.3.12f1`
- XR Interaction Toolkit: `3.3.1`
- OpenXR: `1.16.1`
- Oculus XR Plugin: `4.5.2`
- AR Foundation: `6.3.3`
- Unity UI (UGUI), Visual Scripting, Shader Graph

## Hướng dẫn mở project

1. Mở Unity Hub.
2. Chọn `Add` và trỏ tới thư mục dự án thực tế:
   - `VR_GAME/VR_GAME`
3. Mở project bằng Unity Editor `6000.3.12f1` hoặc phiên bản Unity hỗ trợ gói và OpenXR.
4. Kiểm tra `Project Settings > XR Plug-in Management` và bật OpenXR / Oculus nếu cần.

## Chạy thử

- Mở `MenuScene.unity` để bắt đầu từ menu VR.
- Chuyển đến `IntroScene.unity`, sau đó `GameScene.unity` và `GameSceneIntermédiaire.unity`.
- `EndingScene.unity` là cảnh cuối khi điểm đạt 100.

## Ghi chú

- Dự án có cấu trúc `VR_GAME/VR_GAME` bên trong repository. README này đặt tại root repository để dễ đọc.
- Nếu mở project bị lỗi package, dùng Package Manager để cập nhật hoặc cài lại `XR Interaction Toolkit`, `OpenXR` và `Oculus`.
- Nếu muốn làm sạch repository, nên loại trừ thư mục `Library/` và `Temp/` bằng `.gitignore`.

## Remote repository

Remote hiện tại: `https://github.com/StephenSouth13/VR_GAME_VUI.git`
Branch chính: `main`

---

## Contact

- Author: QuachThanhLong
- Repository gốc: `https://github.com/StephenSouth13/VR_GAME_VUI`
