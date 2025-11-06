Релиз 1.9.1 - 6 ноября 2025 года

Исправлены мелкие ошибки и внесены улучшения в тестовый код.

### Изменено
- Миграция тестов с MSTest на xUnit
- Замена фреймворка тестирования на xUnit с использованием AwesomeAssertions для более удобных и читаемых утверждений
- Рефакторинг больших тестовых файлов на более мелкие и сфокусированные модули
- Обновление атрибутов тестов (`[TestClass]`, `[TestMethod]`) на эквиваленты xUnit (`[Fact]`, `[Trait]`)
- Модернизация методов утверждений с использованием fluent-синтаксиса (например, `Should().Be`, `Should().Throw`)
- Улучшена читаемость и поддерживаемость тестового кода
- Обновление зависимостей проекта с добавлением xUnit и AwesomeAssertions

### Добавлено
- Новые тестовые файлы для лучшей организации:
  - `CharacterSubstitutionElementTests.cs`
  - `CommandElementTests.cs`
  - `EncodingElementTests.cs`
  - `FB2EncoderFallbackBufferTests.cs`
  - `FBEncoderFallbackTests.cs`
  - `FileMetadataTests.cs`
  - `FileOperationResultTests.cs`
  - `FilePropertiesTests.cs`
  - `GenreSubstitutionElementTests.cs`
  - `GenresCollectionTests.cs`
  - `RenameProfileElementTests.cs`
- Файл `packages.config` для управления зависимостями NuGet
- Расширенное покрытие тестами с дополнительными тестовыми случаями

### Удалено
- Старые монолитные тестовые файлы (`ConfigTests.cs`, `FileUtilsTests.cs`) в пользу модульной структуры

### Техническая информация
- Рефакторинг структуры тестов улучшает поддерживаемость и масштабируемость проекта
- Использование современных паттернов тестирования для повышения качества кода
- Улучшена обработка исключений в тестах
