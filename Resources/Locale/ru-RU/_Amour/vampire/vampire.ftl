## Base actions

alerts-vampire-blood-name = Кровавое опьянение
alerts-vampire-blood-desc = Показывает, сколько крови вы выпили. Выпустите клыки и нажмите ЛКМ по цели, чтобы пить кровь.
alerts-vampire-fed-name = Сытость кровью
alerts-vampire-fed-desc = Текущая “сытость” кровью. Пейте кровь, чтобы не голодать.

# Внимание: ключ с опечаткой используется в прототипах.
roles-antag-vamire-name = Вампир
roles-antag-vampire-description = Питайтесь экипажем. Выпустите клыки и пейте их кровь.

vampire-roundend-name = вампир

vampire-drink-start = Вы вонзаете клыки в { CAPITALIZE(THE($target)) }.
vampire-not-enough-blood = Недостаточно крови.
vampire-mouth-covered = Ваш рот закрыт!
vampire-drink-invalid-target = Нельзя пить кровь у вампиров и их треллов.
vampire-target-protected-by-faith = Этого человека защищает вера!
vampire-drink-target-maxed = Вы уже выпили у этой цели { $amount } единиц крови.
vampire-drink-target-hard-max = Вы выпили максимум крови у этой цели ({ $amount } единиц).
vampire-full-power-achieved = Ваша вампирская сущность достигает полной силы!
vampire-umbrae-full-power-fov = Тени склоняются перед вашей волей. Теперь вы видите сквозь стены!

vampire-role-greeting =
    Вы — вампир!
    Жажда крови заставляет вас охотиться на экипаж. Используйте способности, чтобы обращать других.
    Ваши клыки позволяют пить кровь людей. Кровь лечит и открывает новые силы.
    Найдите, чего хотите добиться в эту смену!

# Objectives
objective-issuer-vampire = [color=crimson]Вампир[/color]
objective-condition-drain-title = Выпейте { $count } единиц крови
objective-condition-drain-description = Выпейте { $count } единиц крови у членов экипажа с помощью клыков.
objective-vampire-thrall-obey-master-title = Подчиняйтесь своему хозяину, { $targetName }.

# Class selection action
action-vampire-class-select = Выбрать класс вампира
action-vampire-class-select-desc = Выберите свой подкласс вампира

# Round end statistics
roundend-prepend-vampire-drained-low = Вампиры почти не питались в эту смену, выпив всего { $blood } единиц крови.
roundend-prepend-vampire-drained-medium = Вампиры неплохо поели, выпив { $blood } единиц крови.
roundend-prepend-vampire-drained-high = Вампиры устроили пир, выпив { $blood } единиц крови!
roundend-prepend-vampire-drained-critical = Вампиры сорвались в кровавую вакханалию, выпив невероятные { $blood } единиц крови!
roundend-prepend-vampire-drained = Ни один вампир не смог выпить заметное количество крови в этом раунде.
roundend-prepend-vampire-drained-named = Самым кровожадным вампиром был { $name }, выпив { $number } единиц крови.

# Vampire class selection tooltips
vampire-class-hemomancer-tooltip =
    Хемомант
    Фокус на магии крови и управлении кровью вокруг вас
vampire-class-umbrae-tooltip =
    Умбра
    Фокус на темноте, скрытности, засадах и мобильности
vampire-class-gargantua-tooltip =
    Гаргантюа
    Фокус на выживаемости и уроне в ближнем бою
vampire-class-dantalion-tooltip =
    Данталион
    Фокус на треллах и иллюзиях

# Hemomancer abilities
action-vampire-hemomancer-tendrils-wrong-place = Нельзя применить здесь.
action-vampire-blood-barrier-wrong-place = Нельзя поставить барьеры здесь.
action-vampire-sanguine-pool-already-in = Вы уже в форме кровавой лужи!
action-vampire-sanguine-pool-invalid-tile = Здесь нельзя стать кровавой лужей.
action-vampire-sanguine-pool-enter = Вы превращаетесь в лужу крови!
action-vampire-sanguine-pool-exit = Вы собираетесь обратно из лужи крови!
vampire-space-burn-warning = Жестокий свет пустоты обжигает вашу нежить!
action-vampire-blood-eruption-activated = Вы заставляете кровь вокруг извергнуться шипами!
action-vampire-blood-bringers-rite-not-enough-power = Ваша сила недостаточна (нужно >1000 общей крови и 8 уникальных жертв).
action-vampire-blood-brighters-rite-not-enough-blood = Недостаточно крови для активации ритуала.
action-vampire-blood-bringers-rite-start = Ритуал Кровавого Вестника активирован!
action-vampire-blood-bringers-rite-stop = Ритуал Кровавого Вестника деактивирован.
action-vampire-blood-bringers-rite-stop-blood = Ритуал деактивирован — недостаточно крови.

vampire-locate-result = Ваши чувства ведут к { $target }: { $location }.
vampire-locate-not-same-sector = Этот человек не находится в вашем секторе.
vampire-locate-unknown = Неизвестная зона
vampire-locate-no-targets = На этом секторе не ощущается добычи.
predator-sense-title = Чутьё хищника
vampire-locate-search-placeholder = Поиск...
vampiric-claws-remove-popup = Вы заставляете когти исчезнуть.

# Umbrae abilities
action-vampire-cloak-of-darkness-start = Вы сливаетесь с тенями!
action-vampire-cloak-of-darkness-stop = Вы выходите из теней.
action-vampire-shadow-snare-placed = Вы ставите теневую ловушку.
action-vampire-shadow-snare-wrong-place = Здесь нельзя поставить ловушку.
action-vampire-shadow-snare-scatter = Вы рассеяли теневую ловушку.
vampire-shadow-snare-oldest-removed = Ваша старая ловушка рассеивается.
ent-shadow-snare-ensnare = теневая ловушка
action-vampire-shadow-anchor-returned = Вы возвращаетесь к теневому якорю.
action-vampire-shadow-anchor-installed = Вы закрепляете место в тенях.
action-vampire-shadow-boxing-start = Вы начинаете теневой бокс.
action-vampire-shadow-boxing-stop = Теневой бокс прекращён.
action-vampire-shadow-boxing-ends = Теневой бокс заканчивается.
action-vampire-dark-passage-wrong-place = Тьма здесь непроницаема...
action-vampire-dark-passage-activated = Вы скользите сквозь тьму...
action-vampire-extinguish-activated = Вы поглощаете свет вокруг себя...({ $count })
action-vampire-eternal-darkness-not-enough-blood = У вас закончилась кровь, чтобы поддерживать вечную тьму.
action-vampire-eternal-darkness-start = Вы сотворяете вечную тьму...
action-vampire-eternal-darkness-stop = Вечная тьма рассеивается...

# Dantalion
vampire-enthrall-start = Вы проникаете в разум { CAPITALIZE(THE($target)) }...
vampire-enthrall-success = { CAPITALIZE(THE($target)) } преклоняет колено и становится вашим треллом.
vampire-enthrall-target = Ваш разум подавлен вампирской властью!
vampire-enthrall-limit = Вы не можете контролировать больше треллов.
vampire-enthrall-invalid = Эту цель нельзя поработить.
vampire-thrall-released = Вампирская хватка над вашим разумом ослабевает.

vampire-pacify-invalid = Эту цель нельзя умиротворить.
vampire-pacify-success = { CAPITALIZE(THE($target)) } поддаётся вашей подавляющей безмятежности.
vampire-pacify-target = Сокрушительное спокойствие топит вашу волю к борьбе!

vampire-subspace-swap-thrall = Нельзя менять местами своих треллов.
vampire-subspace-swap-dead = Этот разум вне вашей досягаемости.
vampire-subspace-swap-failed = Подпространственный разлом бесполезно гаснет.
vampire-subspace-swap-success = Пространство изгибается — вы меняетесь местами с { CAPITALIZE(THE($target)) }!
vampire-subspace-swap-target = Реальность искажается, и вас вырывает в новую позицию!

vampire-rally-thralls-success =
    { $count ->
        [one] Ваш зов возвращает трелла на вашу сторону!
       *[other] Ваш зов возвращает { $count } треллов на вашу сторону!
    }
vampire-rally-thralls-none = Ни один из ваших треллов не может ответить на зов.
vampire-thrall-holy-water-freed = Святая вода очищает ваш разум от вампирской власти!
vampire-blood-bond-start = Реки крови связывают вас с треллами.
vampire-blood-bond-stop = Вы ослабляете кровавую связь.
vampire-blood-bond-no-thralls = У вас нет треллов для связи.
vampire-blood-bond-stop-blood = Связь рвётся — у вас недостаточно крови.
action-vampire-not-enough-power = Ваша сила недостаточна (нужно >1000 общей крови и 8 уникальных жертв).

# Gargantua
vampire-blood-swell-start = Ваши мышцы набухают от нечестивой силы!
vampire-blood-swell-end = Кровавая ярость утихает.
vampire-blood-rush-start = Кровь бурлит в ваших жилах!
vampire-blood-rush-end = Ваша сверхъестественная скорость угасает.
vampire-seismic-stomp-activate = Земля содрогается от вашей ярости!
vampire-overwhelming-force-start = Ваше присутствие становится неодолимым.
vampire-overwhelming-force-stop = Вы расслабляете железную хватку.
vampire-overwhelming-force-too-heavy = Этот объект слишком тяжёлый, чтобы сдвинуть!
vampire-overwhelming-force-door-pried = Вы выламываете дверь грубой силой.
vampire-demonic-grasp-hit = Демонический коготь хватает вас!
vampire-demonic-grasp-pull = Коготь тащит вас к вампиру!
vampire-charge-start = Вы срываетесь вперёд неудержимым рывком!
vampire-charge-impact = Вы врезаетесь в { CAPITALIZE(THE($target)) } с сокрушительной силой!
vampire-blood-swell-cancel-shoot = Ваши пальцы не пролезают в спусковую скобу!!
vampire-holy-place-burn = Святая земля обжигает вашу нечестивую плоть!

alerts-vampire-blood-swell-name = Кровавое набухание
alerts-vampire-blood-swell-desc = Ваши мышцы переполняют нечестивая сила.
alerts-vampire-blood-rush-name = Кровавый рывок
alerts-vampire-blood-rush-desc = Сверхъестественная скорость течёт по вашим конечностям.

admin-verb-text-make-vampire = Сделать вампиром
admin-verb-make-vampire = Сделать цель вампиром.

guide-entry-vampire = Вампир
guide-entry-vampire-progression = Прогрессия вампира
guide-entry-vampire-classes = Классы вампира
guide-entry-vampire-counterplay = Противодействие вампиру

mind-role-vampire-name = Роль вампира
ent-vampiric-claws-name = вампирские когти
ent-vampiric-claws-desc = Когти, выкованные из крови. Вытягивают витэ при ударе и исчезают после 15 атак или рассеивания.
ent-vampire-decoy-name = вампирская приманка
ent-vampire-sanguine-pool-name = кровавый сгусток
ent-vampire-sanguine-pool-desc = Разумная лужа вампирской крови.
objective-vampire-survive-name = Выжить
objective-vampire-survive-desc = Я должен выжить любой ценой.
objective-vampire-escape-name = Эвакуироваться живым и без оков.
objective-vampire-escape-desc = Мне нужно покинуть станцию на эвакуационном шаттле, не будучи задержанным.
objective-vampire-kill-random-desc = Любыми средствами не дайте цели добраться до Центкома.
objective-vampire-thrall-obey-name = Подчиняйся своему хозяину
objective-vampire-thrall-obey-desc = Вы обращены в трелла. Следуйте приказам своего хозяина.

action-name-vampire-toggle-fangs = Клыки (переключение)
action-desc-vampire-toggle-fangs = Выпустить или убрать клыки для питья крови у жертв.
action-name-vampire-glare = Взгляд (бесплатно)
action-desc-vampire-glare = Парализует и заставляет молчать ближайшие цели, нанося им урон по выносливости со временем.
action-name-vampire-rejuvenate = Регенерация (бесплатно)
action-desc-vampire-rejuvenate = Мгновенно снимает оглушение и восстанавливает 100 урона по выносливости.
action-name-vampire-rejuvenate-advanced = Регенерация (бесплатно)
action-desc-vampire-rejuvenate-advanced = Мгновенно снимает оглушение, восстанавливает 100 выносливости, очищает вредные реагенты (10u) и лечит урон.
action-name-vampire-choose-class = Выбрать класс вампира
action-desc-vampire-choose-class = Выберите свой подкласс вампира.
action-name-vampire-vampiric-claws = Вампирские когти
action-desc-vampire-vampiric-claws = Создает несбрасываемые кровавые когти. Каждый удар дает +5 крови. 15 ударов. Используйте в руке, чтобы развеять.
action-name-vampire-sanguine-pool = Кровавый сгусток
action-desc-vampire-sanguine-pool = Превращает вас в лужу крови на 8 секунд, позволяя проходить через двери и окна.
action-name-vampire-blood-tendrils = Кровавые щупальца
action-desc-vampire-blood-tendrils = После короткой задержки в области 3x3 вырываются щупальца, отравляя и сильно замедляя жертв.
action-name-vampire-blood-barrier = Кровавый барьер
action-desc-vampire-blood-barrier = Создает 3 кровавых барьера в выбранной точке. Вампиры могут проходить через них.
action-name-vampire-predator-sense = Чутье хищника
action-desc-vampire-predator-sense = Выследите свою добычу, ей негде спрятаться...
action-name-vampire-blood-eruption = Кровавое извержение (100)
action-desc-vampire-blood-eruption = Любая кровь в радиусе 4 тайлов извергается, нанося 50 дробящего урона стоящим на ней.
action-name-vampire-blood-bringers-rite = Ритуал Кровавого Вестника (переключение)
action-desc-vampire-blood-bringers-rite = При активации все рядом начинают сильно кровоточить. Вы пьете их кровь и исцеляетесь.
action-name-vampire-cloak-of-darkness = Покров тьмы (переключение)
action-desc-vampire-cloak-of-darkness = Невидимость и бонус скорости, зависящие от освещения. Сильнее в темноте, слабее при ярком свете.
action-name-vampire-shadow-snare = Теневая ловушка (20)
action-desc-vampire-shadow-snare = Ставит хрупкую теневую ловушку. Наносит урон, ослепляет (20с) и сильно замедляет невампира-гуманоида.
action-name-vampire-shadow-anchor = Теневой якорь (20)
action-desc-vampire-shadow-anchor = Первое применение ставит маяк (2 мин). Повторное мгновенно возвращает к нему и расходует маяк.
action-name-vampire-shadow-boxing = Теневой бокс (50)
action-desc-vampire-shadow-boxing = Прикажите теневым летучим мышам избивать цель. Вы должны оставаться в пределах 4 тайлов.
action-name-vampire-dark-passage = Темный проход (20)
action-desc-vampire-dark-passage = Телепортирует в выбранную точку через тени.
action-name-vampire-extinguish-lights = Погасить свет (0)
action-desc-vampire-extinguish-lights = Уничтожает источники света в радиусе 3 тайлов, давая 5 крови за каждый.
action-name-vampire-eternal-darkness = Вечная тьма (переключение)
action-desc-vampire-eternal-darkness = Окутывает зону вокруг вас тьмой и постепенно понижает температуру тел рядом.
action-name-vampire-enthrall = Порабощение (150)
action-desc-vampire-enthrall = Канал 15 секунд на гуманоиде, чтобы подчинить его вашей воле. Срывается, если кто-то из вас двинется.
action-name-vampire-pacify = Умиротворение (30)
action-desc-vampire-pacify = Наполняет разум жертвы блаженством, пацифицируя ее на 40 секунд.
action-name-vampire-subspace-swap = Подпространственный обмен (30)
action-desc-vampire-subspace-swap = Выберите цель в радиусе 7 тайлов, поменяйтесь с ней местами и замедлите ее на 4 секунды.
action-name-vampire-decoy = Приманка (30)
action-desc-vampire-decoy = Оставляет хрупкую копию, ослепляющую атакующих при повреждении, пока вы скрываетесь в невидимости.
action-name-vampire-rally-thralls = Сбор треллов (100)
action-desc-vampire-rally-thralls = Приказывает треллам в радиусе 7 тайлов снять оглушения, очнуться и восстановить выносливость.
action-name-vampire-blood-bond = Кровавая связь (переключение)
action-desc-vampire-blood-bond = Переключает кровавую связь с ближайшими треллами, перераспределяя урон ценой 2.5 крови в секунду.
action-name-vampire-mass-hysteria = Массовая истерия (70)
action-desc-vampire-mass-hysteria = Насылает ужас на все ближайшие умы (кроме треллов), ослепляя и вызывая галлюцинации на 30 секунд.
action-name-vampire-blood-swell = Кровавый прилив силы (30)
action-desc-vampire-blood-swell = На 30 секунд снижает входящий урон, сокращает длительность станов. Оружие не работает, но растет ближний бой.
action-name-vampire-blood-rush = Кровавый рывок (30)
action-desc-vampire-blood-rush = На 10 секунд удваивает вашу скорость передвижения.
action-name-vampire-seismic-stomp = Сейсмический топот (30)
action-desc-vampire-seismic-stomp = Ударяет по земле, сбивая и отбрасывая существ в радиусе 3 тайлов. Разрушает напольные плитки.
action-name-vampire-overwhelming-force = Непреодолимая сила (переключение)
action-desc-vampire-overwhelming-force = Автоматически вскрывает обесточенные двери. Пока активно, вас нельзя толкать или тянуть. Стоит 5 крови за дверь.
action-name-vampire-demonic-grasp = Демоническая хватка (20)
action-desc-vampire-demonic-grasp = Запускает демоническую руку до 15 тайлов. Обездвиживает цель на 5 секунд и может притянуть в боевом режиме.
action-name-vampire-charge = Таран (30)
action-desc-vampire-charge = Рывок в направлении до препятствия. Существа получают 60 дробящего и отлетают на 5 тайлов. Структуры получают 150 урона.

ent-vampire-effect-blood-tendrils-name = кровавые щупальца
ent-vampire-effect-shadow-punch-name = теневой удар
ent-vampire-effect-blood-barrier-name = кровавый барьер
ent-vampire-effect-blood-barrier-desc = Барьер из застывшей крови, блокирующий проход.
ent-vampire-effect-transformation-out-name = превращение вампира (выход)
ent-vampire-effect-transformation-in-name = превращение вампира (вход)
ent-vampire-effect-blood-eruption-name = кровавое извержение
ent-vampire-effect-drain-beam-name = луч высасывания
ent-vampire-effect-drain-beam-desc = Багровый луч, вытягивающий жизненную силу.
ent-vampire-effect-drain-beam-visual-name = визуал луча высасывания
ent-vampire-effect-drain-beam-visual-desc = Клиентский визуальный эффект луча высасывания вампира.
ent-vampire-effect-shadow-anchor-name = теневой якорь
ent-vampire-effect-shadow-anchor-desc = Пульсирующий узел тени, к которому можно вернуться.
ent-vampire-effect-shadow-snare-name = теневая ловушка
ent-vampire-effect-shadow-snare-desc = Почти невидимая ловушка из уплотненной тьмы.
ent-vampire-effect-shadow-tendrils-name = теневые щупальца
ent-vampire-effect-shadow-tendrils-desc = Темные щупальца, сковывающие ноги.
