# FPS Parkour Oyunu — Kurulum

## Gereksinimler
- **Unity 6** (6000.0.x) — Unity Hub'dan indirilebilir (URP veya Built-in RP)

## Adımlar

### 1. Unity'de Projeyi Aç
1. Unity Hub'ı aç
2. "Add project from disk" → `C:\wamp64\www\Fps-parkour2` klasörünü seç
3. Proje açılana kadar bekle (ilk açılış birkaç dakika sürebilir)

### 2. Sahneyi Oluştur
1. Unity menüsünden: **Tools → Build FPS Parkour Scene**
2. Sahne otomatik olarak kurulur (Player, kameralar, platformlar, HUD)
3. **Play** tuşuna bas — oyun başlar!

## Kontroller
| Tuş | Eylem |
|-----|-------|
| WASD | Hareket |
| Mouse | Kameraya bak |
| Space | Zıpla |
| ESC | İmleç kilidi kaldır |

## Oyun Kuralları
- **50 platform** atlayarak ilerle
- Her **10 platformda bir** sarı checkpoint var → üstüne basınca aktif olur
- Boşluğa düşersen son checkpoint'ten yeniden başlarsın
- Son (50.) platforma (yeşil) ulaşınca **kazanırsın**!

## Dosya Yapısı
```
Assets/
  Scripts/
    PlayerController.cs   — FPS hareket + zıplama + düşme tespiti
    GameManager.cs        — Checkpoint, respawn, kazanma sistemi
    PlatformGenerator.cs  — 50 platformu runtime'da üretir
    CheckpointTrigger.cs  — Platform üstüne basılınca checkpoint kaydeder
    FinishTrigger.cs      — Son platforma ulaşınca kazandırır
  Editor/
    SceneBuilder.cs       — Sahneyi tek tıkla kurar (Editor-only)
```

## Özelleştirme
- `PlatformGenerator` → `totalPlatforms`, `checkpointEvery` değerlerini değiştir
- `PlayerController` → `moveSpeed`, `jumpForce`, `gravity` ayarlanabilir
- `mouseSensitivity` → fare hassasiyeti
