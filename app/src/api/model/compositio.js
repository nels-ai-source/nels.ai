import config from '@/config';
import http from '@/utils/request';

export default {
    book: {
		list: {
            url: `${config.API_URL}/book/getList`,
            name: '获取列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/book/get`,
            name: '获取详情',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/book/create`,
            name: '创建',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/book/update`,
            name: '更新',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        delete: {
            url: `${config.API_URL}/book/delete`,
            name: '删除',
            post: async function (id) {
                return await http.post(`${this.url}?id=${id}`);
            },
        },
    },
    chapter: {
		list: {
            url: `${config.API_URL}/chapter/getList`,
            name: '获取列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/chapter/get`,
            name: '获取详情',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/chapter/create`,
            name: '创建',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/chapter/update`,
            name: '更新',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
        treeList: {
            url: `${config.API_URL}/chapter/getTreeList`,
            name: '获取树列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
    },
    chapterScene: {
		list: {
            url: `${config.API_URL}/chapterScene/getList`,
            name: '获取列表',
            post: async function (params) {
                return await http.post(this.url, params);
            },
        },
        detail: {
            url: `${config.API_URL}/chapterScene/get`,
            name: '获取详情',
            post: async function (params) {
                return await http.post(`${this.url}?id=${params.id}`);
            },
        },
        create: {
            url: `${config.API_URL}/chapterScene/create`,
            name: '创建',
            post: async function (data = {}) {
                return await http.post(this.url, data);
            },
        },
        update: {
            url: `${config.API_URL}/chapterScene/update`,
            name: '更新',
            post: async function (data) {
                return await http.post(`${this.url}?id=${data.id}`, data);
            },
        },
    },
};
