// static classından new ile nesene oluşturulamaz tek bir ortak nokta olarak kullanılır ve oyunun her yerinden bunu duyulması sağlanır.
// farklı noktalardan abone olunur
// her sahneden erişim vardır
// event action yani parametresiz bir olaydır 
// aynı olayı birden fazla script dinleyebilir 
// Skor artırma sistemi: EnemyDied dinler, skor +10 yapar.
// UI güncelleme sistemi: EnemyDied dinler, ekrandaki sayaç günceller.
// Kapı kontrolü: AllEnemiesDead dinler, kapıyı açar.

// Örneğin düşmanın ölüm kodu, “kapı açma” kodunu doğrudan tanımaz.
// Sadece StaticEvents.RaiseEnemyDied() der.
// Kapı açma sistemi ise sadece StaticEvents.EnemyDied += KapıAç; ile olayı dinler.

using System;

public static class StaticEvents
{
    public static event Action EnemyDied;
    public static event Action AllEnemiesDead;
    
// ?.Invoke(); eğer bu olayı dinleyen biri varsa tetikle
// Yani RaiseEnemyDied() çağrıldığında, EnemyDied event’ine abone olmuş tüm fonksiyonlar çalışır.
    public static void RaiseEnemyDied() => EnemyDied?.Invoke(); // Invoke burada RaiseEnemyDied fonksiyonunu tetikliyor çünkü bu fonksiyon EnemyDied event actiona abone
    public static void RaiseAllEnemiesDead() => AllEnemiesDead?.Invoke();
}

// Global state → Çok fazla static event kullanırsan, kontrolsüz “her yerden ulaşılabilen” kod olur.
// Yönetimsiz bırakılırsa sahne değişimlerinde eski abonelikler silinmez → memory leak / ghost event.
// Bu yüzden her OnEnable’da abone ol, OnDisable’da çık: