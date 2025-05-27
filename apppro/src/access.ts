/**
 * @see https://umijs.org/docs/max/access#access
 * */
export default function access(initialState: { currentUser?: API.CurrentUser } | undefined) {
  const { currentUser } = initialState ?? {};
  const permissions = currentUser?.permissions || [];
  const checkPermission = (permissionName: string) => {
    return permissions.includes(permissionName);
  };
  return {
    canAdmin: currentUser && currentUser.roles?.includes('admin'),
    checkAccess: (permissionName: string) => checkPermission(permissionName),
  };
}
export const Permissions = {
  Knowledge: {
    Default: 'Aigc.Knowledge',
    GetList: 'Aigc.Knowledge.GetList',
    Create: 'Aigc.Knowledge.Create',
    Update: 'Aigc.Knowledge.Update',
    Delete: 'Aigc.Knowledge.Delete',
  },
  KnowledgeDocument: {
    Default: 'Aigc.KnowledgeDocument',
    GetList: 'Aigc.KnowledgeDocument.GetList',
    Create: 'Aigc.KnowledgeDocument.Create',
    Update: 'Aigc.KnowledgeDocument.Update',
    Delete: 'Aigc.KnowledgeDocument.Delete',
  },
} as const;
