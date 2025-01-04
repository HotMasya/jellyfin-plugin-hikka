## Про плагін

Цей плагін додає провайдери метаданих та зображень для аніме, манги та ранобе з сайту [Hikka](https://hikka.io/).

## Встановлення

[Див. офіційну документацію для плагінів (англійською)](https://jellyfin.org/docs/general/server/plugins/index.html#installing).

## Збірка

1. Для збірки плагіна вам знадобиться [.NET 8.x](https://dotnet.microsoft.com/download/dotnet/8.0).

2. Зібрати плагін можна наступною командою

```
dotnet publish --configuration Release --output bin
```

3. Перенесіть dll-файл до каталогу `plugins/hikka`, де встановлений Jellyfin. (Можливо вам треба буде створити ці каталоги)

## Реліз

Для релізу цього плагіну ми рекомендуємо [JPRM](https://github.com/oddstr13/jellyfin-plugin-repository-manager) котрий збере цей плагін.
Для отримання додаткової інформації про контекст і про те, як додати упакований zip-файл плагіна до маніфесту плагіна, див. [документація JPRM](https://github.com/oddstr13/jellyfin-plugin-repository-manager).

## Ліцензія

Код цього плагіна та пакунків поширюється на умовах ліцензії GPLv3. Дивіться [LICENSE](./LICENSE) для отримання додаткової інформації.
