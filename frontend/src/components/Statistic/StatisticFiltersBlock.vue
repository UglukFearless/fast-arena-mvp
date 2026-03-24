<template>
    <div class="statistic-filters-block">
        <div class="statistic-filters-block__filter-item">
            <span class="statistic-filters-block__filter-title">Параметр:</span>
            <select>
                <option value="0">Количество побед</option>
                <option value="1">Богатство</option>
                <option value="2">Опыт</option>
            </select>
        </div>
        <div class="statistic-filters-block__filter-item">
            <span class="statistic-filters-block__filter-title">Жив/мёртв:</span>
            <select>
                <option value="0">Все</option>
                <option value="1">Живые</option>
                <option value="2">Мёртвые</option>
            </select>
        </div>
        <div class="statistic-filters-block__filter-item">
            <span class="statistic-filters-block__filter-title">Хазяин:</span>
            <select>
                <option value="0">Все</option>
                <option value="1">Мои</option>
            </select>
        </div>
        <div class="statistic-filters-block__filter-item">
            <span class="statistic-filters-block__filter-title">Порядок:</span>
            <select>
                <option value="0">По возрастанию</option>
                <option value="1">По убыванию</option>
            </select>
        </div>
    </div>
</template>

<script lang="ts" setup>
</script>