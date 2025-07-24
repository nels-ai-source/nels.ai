import { useState, useCallback, useEffect } from 'react';
import { App } from 'antd';
import { useIntl } from '@umijs/max';
import { getAgentList, deleteAgent } from '@/services/aigc/agent';
import type { Agent, AgentFilter } from '@/types/agent';

interface UseAgentListOptions {
    defaultFilter?: Partial<AgentFilter>;
    autoLoad?: boolean;
}

export function useAgentList(options: UseAgentListOptions = {}) {
    const { defaultFilter = {}, autoLoad = true } = options;
    const intl = useIntl();
    const { message } = App.useApp();

    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<Agent[]>([]);
    const [total, setTotal] = useState(0);
    const [filter, setFilter] = useState<AgentFilter>({
        skipCount: 0,
        maxResultCount: 20,
        ...defaultFilter,
    });

    // Load data
    const loadData = useCallback(async (newFilter?: Partial<AgentFilter>) => {
        const queryFilter = newFilter ? { ...filter, ...newFilter } : filter;

        try {
            setLoading(true);
            const result = await getAgentList(queryFilter);
            setData(result.items);
            setTotal(result.totalCount);
            if (newFilter) {
                setFilter(queryFilter);
            }
        } catch (error) {
            message.error(intl.formatMessage({ id: 'common.loadDataFailed' }));
            console.error(error);
        } finally {
            setLoading(false);
        }
    }, [filter, intl, message]);

    // Refresh data
    const refresh = useCallback(() => {
        loadData();
    }, [loadData]);

    // Search
    const search = useCallback((keyword: string) => {
        loadData({ keyword, skipCount: 0 });
    }, [loadData]);

    // Filter data
    const filterData = useCallback((newFilter: Partial<AgentFilter>) => {
        loadData({ ...newFilter, skipCount: 0 });
    }, [loadData]);

    // Pagination
    const paginate = useCallback((page: number, pageSize: number) => {
        loadData({
            skipCount: (page - 1) * pageSize,
            maxResultCount: pageSize,
        });
    }, [loadData]);

    // Delete item
    const deleteItem = useCallback(async (id: string) => {
        try {
            await deleteAgent(id);
            message.success(intl.formatMessage({ id: 'common.deleteSuccess' }));
            refresh();
        } catch (error) {
            message.error(intl.formatMessage({ id: 'common.deleteFailed' }));
            console.error(error);
        }
    }, [refresh, intl, message]);

    useEffect(() => {
        if (autoLoad) {
            loadData();
        }
    }, []);

    return {
        loading,
        data,
        total,
        filter,
        loadData,
        refresh,
        search,
        filterData,
        paginate,
        deleteItem,
    };
}