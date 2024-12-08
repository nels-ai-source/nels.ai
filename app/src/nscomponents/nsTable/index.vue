<template>
    <div class="crud2-container">
        <!-- 条件查询表单 -->
        <FormComponent v-if="filter" :config="filter" @search="handleSearch" />

        <!-- CRUD表格 -->
        <el-table v-loading="loading" :data="tableData" @selection-change="handleSelectionChange" :row-key="primaryField">
            <!-- 动态渲染列 -->
            <template v-for="col in columns" :key="col.id">
                <el-table-column v-bind="col.props" :prop="col.name" :label="col.title" />
                <!-- 如果是操作列，特殊处理 -->
                <OperationColumn v-if="col.type === 'operation'" :buttons="col.buttons" />
            </template>
        </el-table>

        <!-- 分页组件 -->
        <el-pagination v-if="footerToolbar?.length" v-model:current-page="currentPage" v-model:page-size="pageSize" :total="total" layout="total, prev, pager, next, jumper" @current-change="handleCurrentChange" @size-change="handleSizeChange" />
    </div>
</template>
  
  <script setup>
import { ref, onMounted } from 'vue';
import FormComponent from './FormComponent.vue'; // 自定义表单组件
import OperationColumn from './OperationColumn.vue'; // 操作按钮组件
import { fetchData } from './api'; // 假设这是获取数据的函数

const props = defineProps({
    // ...所有传入的属性
});

const loading = ref(false);
const tableData = ref([]);
const currentPage = ref(1);
const pageSize = ref(props.loadType === 'pagination' ? 10 : 0);
const total = ref(0);
const selectedItems = ref([]);

// 初始化数据加载
onMounted(() => {
    loadData();
});

// 加载数据
async function loadData() {
    loading.value = true;
    try {
        const params = {
            ...buildParams(),
            page: currentPage.value,
            size: pageSize.value,
        };
        const result = await fetchData(props.api.url, {
            method: props.api.method,
            data: params,
        });
        tableData.value = result.data || [];
        total.value = result.total || 0;
    } finally {
        loading.value = false;
    }
}

// 构建请求参数，包括查询条件和分页信息
function buildParams() {
    // ...根据filter等属性构建请求参数
}

// 处理搜索事件
function handleSearch() {
    // ...更新查询条件，然后重新加载数据
}

// 分页处理
function handleCurrentChange(page) {
    currentPage.value = page;
    loadData();
}

function handleSizeChange(size) {
    pageSize.value = size;
    loadData();
}

// 行选择变化处理
function handleSelectionChange(selection) {
    selectedItems.value = selection;
}
</script>